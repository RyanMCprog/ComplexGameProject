using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplate : MonoBehaviour
{
    public GameObject[] TopRooms;
    public GameObject[] BottomRooms;
    public GameObject[] NorthRooms;
    public GameObject[] SouthRooms;
    public GameObject[] WestRooms;
    public GameObject[] EastRooms;

    public GameObject[] GameTesters;

    public GameObject[] TopDeadEnds;
    public GameObject[] BottomDeadEnds;
    public GameObject[] NorthDeadEnds;
    public GameObject[] SouthDeadEnds;
    public GameObject[] WestDeadEnds;
    public GameObject[] EastDeadEnds;

    [HideInInspector]
    public bool stopBuild = false;

    [HideInInspector]
    public bool allowDeadEnds = false;

    [HideInInspector]
    public int RoomLimit = 10;

    //private int currentRoomLimit;

    //public int DeadEndLimit = 20;
    //private int currentDeadEnds;
    //public bool workingOnDeadEnds = false;

    private void Start()
    {
        //currentRoomLimit = RoomLimit;
        //stopBuild = false;
    }
    public void CurrentRooms()
    {
        RoomLimit -= 1;
        //currentRoomLimit -= 1;
        if(RoomLimit <= 0)//currentRoomLimit <= 0)
        {
            stopBuild = true;
            Debug.Log("StopBuild is now true");
            //workingOnDeadEnds = true;
        }
    }

    
}
