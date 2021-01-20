using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public RoomTemplate values;
    public int NumberOfInnerRooms = 3;
    // Start is called before the first frame update
    void Start()
    {
        values.stopBuild = false;
        values.allowDeadEnds = true;
        values.RoomLimit = NumberOfInnerRooms;
    }
}
