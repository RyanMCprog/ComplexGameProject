using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class SocketExample2 : MonoBehaviour
{
    const int portnumber = 3456;
    //UI field
    public TMPro.TMP_InputField ipAddress;
    public TMPro.TMP_InputField messageToSend;
    public TMPro.TextMeshProUGUI messageLog;

    //Socket things
    Socket listener;
    byte[] buffer = new byte[4];
    Socket acceptedSocket;

    //Misc
    string messageLogText = "";

    string GetHostIP()
    {
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        foreach(var ip in host.AddressList)
        {
            if(ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system");
    }

    /// <summary>
    /// By deflaut lets the target IP address to the proper IP of this machine for same PC testing.
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        ipAddress.text = GetHostIP();
    }

    public void SendTextMessage()
    {
        try
        {
            IPAddress ipAdder = IPAddress.Parse(ipAddress.text);
            IPEndPoint localEndPoint = new IPEndPoint(ipAdder, portnumber);
            //Create TCP/IP Socket using Socket class constructor
            Socket sender = new Socket(ipAdder.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //Connect Socket to the remote endpoint using method Connect()
                sender.Connect(localEndPoint);
                //we print EndPoint information that we are connected
                Debug.Log($"Socket connected to -> {sender.RemoteEndPoint.ToString()}");

                //creation of message that we will send to Server
                //no need to worry about an EOF marker
                byte[] messageSent = Encoding.ASCII.GetBytes(messageToSend.text);

                var fullPacket = new List<byte>();
                fullPacket.AddRange(BitConverter.GetBytes(messageSent.Length));
                fullPacket.AddRange(messageSent);

                //send the message to the server we are currently connected to
                sender.Send(fullPacket.ToArray());

                //Get a handshaked response
                //Data buffer
                byte[] messageReceived = new byte[1024];
                int byteRecv = sender.Receive(messageReceived);
                Debug.Log($"Message from Server -> {Encoding.ASCII.GetString(messageReceived, 0, byteRecv)}");
                if (GetHostIP() != ipAddress.text)
                    messageLog.text = Encoding.ASCII.GetString(messageReceived, 0, byteRecv) + messageLog.text;

                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
            catch (Exception e)
            {
                Debug.Log($"Expection : {e.ToString()}");
            }
        }
        catch (Exception e)
        {
            Debug.Log($"Expection : {e.ToString()}");
        }
    }

    public void ExecuteServer()
    {
        StartCoroutine(UpdateMessageLog());
        IPAddress ipAdder = IPAddress.Parse(GetHostIP());
        IPEndPoint localEndPoint = new IPEndPoint(ipAdder, portnumber);

        //Data buffer for incoming data
        byte[] bytes = new Byte[1024]; //does the Byte need to be capitial 

        //Create a TCP/IP socket
        listener = new Socket(ipAdder.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        //link for binds in Sharepoint 
        listener.Bind(localEndPoint);

        //link for socket listen in sharepoint
        listener.Listen(10);
        //link for socket beginaccept in sharepoint
        listener.BeginAccept(AcceptCallback, listener);
    }

    void AcceptCallback(IAsyncResult ar)
    {
        //reset the buffer down to the 4 bytes to get the length of the packet
        buffer = new byte[4];
        Debug.Log("Socket Conection Accepted by Server");

        //link for socket endAccept in sharepoint
        acceptedSocket = listener.EndAccept(ar);
        //this provides a very basic message handler
        //link for socket beginReceive in sharepoint
        acceptedSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
    }

    void ReceiveCallback(IAsyncResult ar)
    {
        Debug.Log("Begin processing incoming data");

        if(acceptedSocket.EndReceive(ar) > 1)
        {
            //We receive the byte length of the incoming packet/message in the first 4 bytes so we can easily adjust the buffer here
            buffer = new byte[BitConverter.ToInt32(buffer, 0)];

            Debug.Log($"Expecting {buffer.Length} bytes from connection");
            //now get the actual data
            //link for socket receive in sharepoint
            acceptedSocket.Receive(buffer, buffer.Length, SocketFlags.None);

            //output the result (and send it back to the client as a poormans handshake)
            string data = Encoding.Default.GetString(buffer);
            messageLogText = data + "\n" + messageLogText;
            acceptedSocket.Send(buffer);
            //wrap up and go back to listening
            acceptedSocket.Shutdown(SocketShutdown.Both);
            acceptedSocket.Close();
            listener.BeginAccept(AcceptCallback, listener);
        }
        else
        {
            Disconnect();
        }
    }

    void Disconnect()
    {
        acceptedSocket.Disconnect(true);
    }

    IEnumerator UpdateMessageLog()
    {
        while(true)
        {
            messageLog.text = messageLogText;
            yield return new WaitForSeconds(.25f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
