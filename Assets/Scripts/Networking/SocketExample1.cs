using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.VersionControl;

public class SocketExample1 : MonoBehaviour
{
    const int PORTNUMBER = 3456;

    //UI fields
    public TMPro.TMP_InputField ipAddress;
    public TMPro.TMP_InputField messageToSend;
    public TMPro.TextMeshProUGUI messageLog;

    //Sockets
    Socket listener;
    Socket handler;

    //Multithreading
    Mutex mutex = new Mutex();
    private Thread SocketThread;

    //misc
    public bool keepReading;
    string messageLogText = "";

    /// <summary>
    /// Gets the local Ip address of the PC being executed on (this PC)
    /// </summary>
    /// <returns></returns>

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
    /// By deflaut, set the target IP address to the IP of this machine for same PC testing.
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
            IPEndPoint remoteEndPoint = new IPEndPoint(ipAdder, PORTNUMBER);

            //instantiate a TCP/IP Socket using Socket class
            Socket sender = new Socket(ipAdder.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //connect Socket to the remote endpoint using method connect()
                sender.Connect(remoteEndPoint);
                //we print Endpoint informationthat we are connected
                Debug.Log($"Socket connected to -> {sender.RemoteEndPoint.ToString()}");
                //Creation of message that we will send to server
                byte[] messageSent = Encoding.ASCII.GetBytes(messageToSend.text + "<EOF>");
                int byteSent = sender.Send(messageSent);
                //data buffer
                byte[] messageReceived = new byte[1024];
                //we receive the message using the method receive().
                //returns number of bytes received, that we'll use to convert them to string
                int byteRecv = sender.Receive(messageReceived);
                Debug.Log($"Message from Server -> {Encoding.ASCII.GetString(messageReceived, 0, byteRecv)}");
                //if this is running on a seperate machine to the server then echo the response from the server on screen
                if (GetHostIP() != ipAddress.text)
                    messageLog.text = Encoding.ASCII.GetString(messageReceived, 0, byteRecv) + messageLog.text;
                //Shutdown and close the socket
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
            catch (Exception e)
            {
                Debug.Log($"Exception Processing Data: {e.ToString()}");
            }
        }
        catch(Exception e)
        {
            Debug.Log($"Exception: {e.ToString()}");
        }
    }

    /// <summary>
    /// Start up the server thread.
    /// </summary>
    public void ExecuteServer()
    {
        StartCoroutine(UpdateMessageLog());
        SocketThread = new System.Threading.Thread(ThreadedServer);
        SocketThread.IsBackground = true;
        SocketThread.Start();
    }

    /// <summary>
    /// server thread, monitors the port for incoming data and updates the messageLogText string for outputting
    /// </summary>
    public void ThreadedServer()
    {
        IPAddress ipAddr = IPAddress.Parse(GetHostIP());
        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, PORTNUMBER);
        string data;
        //Data buffer for incoming data
        byte[] bytes = new byte[1024];
        //create a TCP/IP socket
        listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            //Bind the socket to the local endpoint and
            //listen for incoming connections
            listener.Bind(localEndPoint);
            listener.Listen(10);
            //Start listening for connections.
            while(true)
            {
                keepReading = true;
                //program is suspended while waiting for an incoming connection
                Debug.Log("Waiting for connection"); //it works
                handler = listener.Accept();
                Debug.Log("Client Connected");
                data = null;
                //an incoming connection needs to be processed
                while(keepReading)
                {
                    mutex.WaitOne();
                    string tmp = messageLog.text;
                    mutex.ReleaseMutex();
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    Debug.Log("Received from Server");

                    if(bytesRec <= 0)
                    {
                        keepReading = false;
                        handler.Disconnect(true);
                        break;
                    }

                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if(data.IndexOf("<EOF>") > -1)
                    {
                        mutex.WaitOne();
                        messageLogText = data.Replace("<EOF>", "\n") + messageLogText;
                        mutex.ReleaseMutex();
                        Debug.Log(data);
                        //Should send a response as well
                        handler.Send(Encoding.ASCII.GetBytes(data.Replace("<EOF>", "\n")));
                        break;
                    }
                    System.Threading.Thread.Sleep(1);
                }
                System.Threading.Thread.Sleep(1);
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    /// <summary>
    /// Updates the messagelog output for the server side
    /// </summary>
    IEnumerator UpdateMessageLog()
    {
        while (true)
        {
            messageLog.text = messageLogText;
            yield return new WaitForSeconds(.25f);
        }
    }
    void stopServer()
    {
        keepReading = false;

        //stop thread
        if(SocketThread != null)
        {
            SocketThread.Abort();
        }

        if(handler != null && handler.Connected)
        {
            handler.Disconnect(false);
            Debug.Log("Disconnected!");
        }
    }

    private void OnDisable()
    {
        stopServer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
