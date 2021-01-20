using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class DungeonSpawner : MonoBehaviour
{
    private int rand;
    public RoomTemplate templates;

    public bool Top;
    public bool Bottom;
    public bool North;
    public bool South;
    public bool West;
    public bool East;

    public float startingTime = 5f;
    private float currentTime;

    //room information
    private float GridsOffset;
    private int GridX;
    private int GridY;
    private int GridZ;
    private Vector3 GridsOrigin;

    private bool hasRoomSpawn = false;
    private bool isDestroyed = false;

    private IsTIle CollidedTile;

    public GameObject testRock;
    //public GameObject OverlapTester;
    //private TestOverlap TestDirection;

    // Start is called before the first frame update
    void Awake()
    {
        //TestDirection = OverlapTester.GetComponent<TestOverlap>();
        //SpawnOverlapTester();
        currentTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!hasRoomSpawn)
        {
            if (currentTime <= 0)
            {
                Debug.Log("Spawning Room");
                SpawnRoom();
                hasRoomSpawn = true;
                currentTime = startingTime;
            }
            else
            {
                currentTime -= Time.deltaTime;
            }
        }
        
    }

    void SpawnRoom() //fix problem where this freezes unity(DONE)
    {
        if(!templates.stopBuild)
        {
            if (Top)
            {
                rand = Random.Range(0, templates.BottomRooms.Length);
                GridsOffset = templates.BottomRooms[rand].GetComponent<SpawnObject>().gridSpacingOffset;
                GridX = templates.BottomRooms[rand].GetComponent<SpawnObject>().gridX;
                GridY = templates.BottomRooms[rand].GetComponent<SpawnObject>().gridY;
                GridZ = templates.BottomRooms[rand].GetComponent<SpawnObject>().gridZ;
                templates.BottomRooms[rand].GetComponent<SpawnObject>().gridOrigin = transform.position;
                GridsOrigin = templates.BottomRooms[rand].GetComponent<SpawnObject>().gridOrigin; //NEW
                TestOverlaps();
                if (isDestroyed)
                {
                    return;
                }
                GameObject spawn = Instantiate(templates.BottomRooms[rand], transform.position += new Vector3(0, GridY * GridsOffset, 0), Quaternion.identity);
                templates.CurrentRooms();
                Destroy(gameObject); Debug.Log("OK for Spawning\n" + spawn.name, spawn);
                return;
            }
            else if (Bottom)
            {
                rand = Random.Range(0, templates.TopRooms.Length);
                GridsOffset = templates.TopRooms[rand].GetComponent<SpawnObject>().gridSpacingOffset;
                GridX = templates.TopRooms[rand].GetComponent<SpawnObject>().gridX;
                GridY = templates.TopRooms[rand].GetComponent<SpawnObject>().gridY;
                GridZ = templates.TopRooms[rand].GetComponent<SpawnObject>().gridZ;
                templates.TopRooms[rand].GetComponent<SpawnObject>().gridOrigin = transform.position -= new Vector3(0, (GridY - 1) * GridsOffset, 0);
                GridsOrigin = templates.TopRooms[rand].GetComponent<SpawnObject>().gridOrigin; //NEW
                TestOverlaps();
                if (isDestroyed)
                {
                    return;
                }
                GameObject spawn = Instantiate(templates.TopRooms[rand], templates.TopRooms[rand].GetComponent<SpawnObject>().gridOrigin, Quaternion.identity);
                templates.CurrentRooms();
                Destroy(gameObject); Debug.Log("OK for Spawning\n" + spawn.name, spawn);
                return;
            }
            else if (North)
            {
                rand = Random.Range(0, templates.SouthRooms.Length);
                GridsOffset = templates.SouthRooms[rand].GetComponent<SpawnObject>().gridSpacingOffset;
                GridX = templates.SouthRooms[rand].GetComponent<SpawnObject>().gridX;
                GridY = templates.SouthRooms[rand].GetComponent<SpawnObject>().gridY;
                GridZ = templates.SouthRooms[rand].GetComponent<SpawnObject>().gridZ;
                templates.SouthRooms[rand].GetComponent<SpawnObject>().gridOrigin = transform.position -= new Vector3(0, 0, (GridZ - 1) * GridsOffset);
                GridsOrigin = templates.SouthRooms[rand].GetComponent<SpawnObject>().gridOrigin; //NEW
                TestOverlaps();
                if (isDestroyed)
                {
                    return;
                }
                GameObject spawn = Instantiate(templates.SouthRooms[rand], templates.SouthRooms[rand].GetComponent<SpawnObject>().gridOrigin, Quaternion.identity);
                templates.CurrentRooms();
                Destroy(gameObject); Debug.Log("OK for Spawning\n" + spawn.name, spawn);
                return;
            }
            else if (South)
            {
                rand = Random.Range(0, templates.NorthRooms.Length);
                GridsOffset = templates.NorthRooms[rand].GetComponent<SpawnObject>().gridSpacingOffset;
                GridX = templates.NorthRooms[rand].GetComponent<SpawnObject>().gridX;
                GridY = templates.NorthRooms[rand].GetComponent<SpawnObject>().gridY;
                GridZ = templates.NorthRooms[rand].GetComponent<SpawnObject>().gridZ;
                templates.NorthRooms[rand].GetComponent<SpawnObject>().gridOrigin = transform.position;
                GridsOrigin = templates.NorthRooms[rand].GetComponent<SpawnObject>().gridOrigin; //NEW
                TestOverlaps();
                if (isDestroyed)
                {
                    return;
                }
                GameObject spawn = Instantiate(templates.NorthRooms[rand], templates.NorthRooms[rand].GetComponent<SpawnObject>().gridOrigin, Quaternion.identity);
                templates.CurrentRooms();
                Destroy(gameObject); Debug.Log("OK for Spawning\n" + spawn.name, spawn);
                return;
            }
            else if (West)
            {
                rand = Random.Range(0, templates.EastRooms.Length);
                GridsOffset = templates.EastRooms[rand].GetComponent<SpawnObject>().gridSpacingOffset;
                GridX = templates.EastRooms[rand].GetComponent<SpawnObject>().gridX;
                GridY = templates.EastRooms[rand].GetComponent<SpawnObject>().gridY;
                GridZ = templates.EastRooms[rand].GetComponent<SpawnObject>().gridZ;
                templates.EastRooms[rand].GetComponent<SpawnObject>().gridOrigin = transform.position -= new Vector3((GridX - 1) * GridsOffset, 0, 0);
                GridsOrigin = templates.EastRooms[rand].GetComponent<SpawnObject>().gridOrigin; //NEW
                TestOverlaps();
                if (isDestroyed)
                {
                    return;
                }
                GameObject spawn = Instantiate(templates.EastRooms[rand], templates.EastRooms[rand].GetComponent<SpawnObject>().gridOrigin, Quaternion.identity);
                templates.CurrentRooms();
                Destroy(gameObject); Debug.Log("OK for Spawning\n" + spawn.name, spawn);
                return;
            }
            else if (East)
            {
                rand = Random.Range(0, templates.WestRooms.Length);
                GridsOffset = templates.WestRooms[rand].GetComponent<SpawnObject>().gridSpacingOffset;
                GridX = templates.WestRooms[rand].GetComponent<SpawnObject>().gridX;
                GridY = templates.WestRooms[rand].GetComponent<SpawnObject>().gridY; 
                GridZ = templates.WestRooms[rand].GetComponent<SpawnObject>().gridZ;
                templates.WestRooms[rand].GetComponent<SpawnObject>().gridOrigin = transform.position;
                GridsOrigin = templates.WestRooms[rand].GetComponent<SpawnObject>().gridOrigin; //NEW
                TestOverlaps();
                if (isDestroyed)
                {
                    return;
                }
                GameObject spawn = Instantiate(templates.WestRooms[rand], templates.EastRooms[rand].GetComponent<SpawnObject>().gridOrigin, Quaternion.identity);
                templates.CurrentRooms();
                Destroy(gameObject); Debug.Log("OK for Spawning\n" + spawn.name, spawn);
                return;
            }
        }
        else if(templates.allowDeadEnds) //this section has the error involving the array and spawns dead ends
        {
            //when room limit is meet
            if (Top)
            {
                rand = Random.Range(0, templates.BottomDeadEnds.Length);
                GridsOffset = templates.BottomDeadEnds[rand].GetComponent<SpawnObject>().gridSpacingOffset;
                GridX = templates.BottomDeadEnds[rand].GetComponent<SpawnObject>().gridX;
                GridY = templates.BottomDeadEnds[rand].GetComponent<SpawnObject>().gridY;
                GridZ = templates.BottomDeadEnds[rand].GetComponent<SpawnObject>().gridZ;
                templates.BottomDeadEnds[rand].GetComponent<SpawnObject>().gridOrigin = transform.position;
                GridsOrigin = templates.BottomDeadEnds[rand].GetComponent<SpawnObject>().gridOrigin; //NEW
                TestOverlaps();
                if (isDestroyed)
                {
                    return;
                }
                GameObject spawn = Instantiate(templates.BottomDeadEnds[rand], templates.BottomDeadEnds[rand].GetComponent<SpawnObject>().gridOrigin, Quaternion.identity);
                Destroy(gameObject); Debug.Log("OK for Spawning\n" + spawn.name, spawn);
                return;
            }
            else if (Bottom)
            {
                rand = Random.Range(0, templates.TopDeadEnds.Length);
                GridsOffset = templates.TopDeadEnds[rand].GetComponent<SpawnObject>().gridSpacingOffset;
                GridX = templates.TopDeadEnds[rand].GetComponent<SpawnObject>().gridX;
                GridY = templates.TopDeadEnds[rand].GetComponent<SpawnObject>().gridY;
                GridZ = templates.TopDeadEnds[rand].GetComponent<SpawnObject>().gridZ;
                templates.TopDeadEnds[rand].GetComponent<SpawnObject>().gridOrigin = transform.position -= new Vector3(0, (GridY - 1) * GridsOffset, 0);
                GridsOrigin = templates.TopDeadEnds[rand].GetComponent<SpawnObject>().gridOrigin; //NEW
                TestOverlaps();
                if (isDestroyed)
                {
                    return;
                }
                GameObject spawn = Instantiate(templates.TopDeadEnds[rand], templates.TopDeadEnds[rand].GetComponent<SpawnObject>().gridOrigin, Quaternion.identity);
                Destroy(gameObject); Debug.Log("OK for Spawning\n" + spawn.name, spawn);
                return;
            }
            else if (North)
            {
                rand = Random.Range(0, templates.SouthDeadEnds.Length);
                GridsOffset = templates.SouthDeadEnds[rand].GetComponent<SpawnObject>().gridSpacingOffset;
                GridX = templates.SouthDeadEnds[rand].GetComponent<SpawnObject>().gridX;
                GridY = templates.SouthDeadEnds[rand].GetComponent<SpawnObject>().gridY;
                GridZ = templates.SouthDeadEnds[rand].GetComponent<SpawnObject>().gridZ;
                templates.SouthDeadEnds[rand].GetComponent<SpawnObject>().gridOrigin = transform.position -= new Vector3(0, 0, (GridZ - 1) * GridsOffset);
                GridsOrigin = templates.SouthDeadEnds[rand].GetComponent<SpawnObject>().gridOrigin; //NEW
                TestOverlaps();
                if (isDestroyed)
                {
                    return;
                }
                GameObject spawn = Instantiate(templates.SouthDeadEnds[rand], templates.SouthDeadEnds[rand].GetComponent<SpawnObject>().gridOrigin, Quaternion.identity);
                Destroy(gameObject); Debug.Log("OK for Spawning\n" + spawn.name, spawn);
                return;
            }
            else if (South)
            {
                rand = Random.Range(0, templates.NorthDeadEnds.Length);
                GridsOffset = templates.NorthDeadEnds[rand].GetComponent<SpawnObject>().gridSpacingOffset;
                GridX = templates.NorthDeadEnds[rand].GetComponent<SpawnObject>().gridX;
                GridY = templates.NorthDeadEnds[rand].GetComponent<SpawnObject>().gridY;
                GridZ = templates.NorthDeadEnds[rand].GetComponent<SpawnObject>().gridZ;
                templates.NorthDeadEnds[rand].GetComponent<SpawnObject>().gridOrigin = transform.position;
                GridsOrigin = templates.NorthDeadEnds[rand].GetComponent<SpawnObject>().gridOrigin; //NEW
                TestOverlaps();
                if (isDestroyed)
                {
                    return;
                }
                GameObject spawn = Instantiate(templates.NorthDeadEnds[rand], templates.NorthDeadEnds[rand].GetComponent<SpawnObject>().gridOrigin, Quaternion.identity);
                Destroy(gameObject); Debug.Log("OK for Spawning\n" + spawn.name, spawn);
                return;
            }
            else if (West)
            {
                rand = Random.Range(0, templates.EastDeadEnds.Length);
                GridsOffset = templates.EastDeadEnds[rand].GetComponent<SpawnObject>().gridSpacingOffset;
                GridX = templates.EastDeadEnds[rand].GetComponent<SpawnObject>().gridX;
                GridY = templates.EastDeadEnds[rand].GetComponent<SpawnObject>().gridY;
                GridZ = templates.EastDeadEnds[rand].GetComponent<SpawnObject>().gridZ;
                templates.EastDeadEnds[rand].GetComponent<SpawnObject>().gridOrigin = transform.position -= new Vector3((GridX - 1) * GridsOffset, 0, 0);
                GridsOrigin = templates.EastDeadEnds[rand].GetComponent<SpawnObject>().gridOrigin; //NEW
                TestOverlaps();
                if (isDestroyed)
                {
                    return;
                }
                GameObject spawn = Instantiate(templates.EastDeadEnds[rand], templates.EastDeadEnds[rand].GetComponent<SpawnObject>().gridOrigin, Quaternion.identity);
                Destroy(gameObject); Debug.Log("OK for Spawning\n" + spawn.name, spawn);
                return;
            }
            else if (East)
            {
                rand = Random.Range(0, templates.WestDeadEnds.Length);
                GridsOffset = templates.WestDeadEnds[rand].GetComponent<SpawnObject>().gridSpacingOffset;
                GridX = templates.WestDeadEnds[rand].GetComponent<SpawnObject>().gridX;
                GridY = templates.WestDeadEnds[rand].GetComponent<SpawnObject>().gridY;
                GridZ = templates.WestDeadEnds[rand].GetComponent<SpawnObject>().gridZ;
                templates.WestDeadEnds[rand].GetComponent<SpawnObject>().gridOrigin = transform.position;
                GridsOrigin = templates.WestDeadEnds[rand].GetComponent<SpawnObject>().gridOrigin; //NEW
                TestOverlaps();
                if (isDestroyed)
                {
                    return;
                }
                GameObject spawn = Instantiate(templates.WestDeadEnds[rand], templates.WestDeadEnds[rand].GetComponent<SpawnObject>().gridOrigin, Quaternion.identity);
                Destroy(gameObject); Debug.Log("OK for Spawning\n" + spawn.name, spawn);
                return;
            }
        }
        Debug.Log("Dungeon Done");
        templates.allowDeadEnds = false;
        Destroy(gameObject);
    }

    void TestOverlaps()
    {
        bool success = Physics.OverlapBox(new Vector3(GridX / 2 * GridsOffset, GridY / 2 * GridsOffset, GridZ / 2 * GridsOffset) + GridsOrigin, new Vector3(GridX / 2, GridY / 2, GridZ / 2), Quaternion.identity).Length > 0;
        if (success)
        {
            //Instantiate(testRock, new Vector3(GridX / 2 * GridsOffset, GridY / 2 * GridsOffset, GridZ / 2 * GridsOffset) + GridsOrigin, Quaternion.identity);
            isDestroyed = true;
            Destroy(gameObject);
            Debug.Log("Collision");
        }
    }

    void RoomDetails(GameObject[] roomsArray, int RandRoom)
    {

    }
}
