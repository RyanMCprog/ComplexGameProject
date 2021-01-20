using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] Tile;
    //public GameObject[] TrapTile;
    //public GameObject DoorTile;
    public int gridX = 30;
    public int gridZ = 30;
    public int gridY = 30;
    public float gridSpacingOffset = 1f;
    public Vector3 gridOrigin = Vector3.zero;

    //doors of the Grid
    public int DoorHeightStart = 9;
    public int DoorHeightEnd = 11;

    //public int DoorYStart = 9;
    //public int DoorYEnd = 11;

    public int DoorWidthStart = 9;
    public int DoorWidthEnd = 11;

    //For Each Exit
    
    List<GameObject> CurrentGrid;
    public bool TopExit = false;
    public bool BottomExit = false;
    public bool NorthWallExit = false;
    public bool SouthWallExit = false;
    public bool WestWallExit = false;
    public bool EastWallExit = false;

    
    public int roomTesterOffset = 1;
    public RoomTemplate Testers;

    //Room Light
    public GameObject GameLight;

    //Spawns in hole to stop overlap
    public GameObject Hole;

    // Start is called before the first frame update
    void Start()
    {
        SpawnFloorGrid();
        SpawnWallGrids();
        SpawnCeiling();
        SpawnLights();
        OnRoomSpawn();
    }

    void SpawnTiles(Vector3 spawnPosition, Quaternion RotationSpawn)
    {
        
        int rand = Random.Range(0, Tile.Length);
        CurrentGrid.Add(Instantiate(Tile[rand], spawnPosition, RotationSpawn));
        
    }
    //rooms floor
    void SpawnFloorGrid()
    {
        
        CurrentGrid = new List<GameObject>();
        for(int x = 0; x < gridX; x++)
        {
            for(int z = 0; z < gridZ; z++)
            {
                Vector3 spawnPos = new Vector3(x * gridSpacingOffset, 0, z * gridSpacingOffset) + gridOrigin;
                SpawnTiles(spawnPos, Quaternion.identity);
            }
        }
        if(BottomExit)
        {
            MakeHoles();
            Instantiate(Testers.GameTesters[0], gridOrigin - new Vector3(0, roomTesterOffset, 0), Quaternion.identity);
        }
    }
    //rooms four walls
    void SpawnWallGrids()
    {
        //NorthWall
        CurrentGrid = new List<GameObject>();
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                Vector3 spawnPos = new Vector3(x * gridSpacingOffset, y * gridSpacingOffset, 0) + gridOrigin;
                SpawnTiles(spawnPos, Quaternion.identity);
            }
        }
        if (NorthWallExit)
        {
            MakeHoles();
            Instantiate(Testers.GameTesters[1], gridOrigin - new Vector3(0, 0, roomTesterOffset), Quaternion.identity);
        }

        //WestWall
        CurrentGrid = new List<GameObject>();
        for (int z = 0; z < gridZ; z++)
        {
            for (int y = 0; y < gridY; y++)
            {
                Vector3 spawnPos = new Vector3(0, y * gridSpacingOffset, z * gridSpacingOffset) + gridOrigin;
                SpawnTiles(spawnPos, Quaternion.identity);
            }
        }
        if (WestWallExit)
        {
            MakeHoles();
            Instantiate(Testers.GameTesters[2], gridOrigin - new Vector3(roomTesterOffset, 0, 0), Quaternion.identity);
        }

        //EastWall
        CurrentGrid = new List<GameObject>();
        for (int z = 0; z < gridZ; z++)
        {
            for (int y = 0; y < gridY; y++)
            {
                Vector3 spawnPos = new Vector3(0, y * gridSpacingOffset, z * gridSpacingOffset) + gridOrigin + new Vector3((gridX - 1) * gridSpacingOffset,0,0);
                SpawnTiles(spawnPos, Quaternion.identity);
            }
        }
        if (EastWallExit)
        {
            MakeHoles();
            Instantiate(Testers.GameTesters[3], gridOrigin + new Vector3(roomTesterOffset, 0, 0) + new Vector3((gridX - 1) * gridSpacingOffset, 0, 0), Quaternion.identity);
        }

        //southWall
        CurrentGrid = new List<GameObject>();
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                Vector3 spawnPos = new Vector3(x * gridSpacingOffset, y * gridSpacingOffset, 0) + gridOrigin + new Vector3(0, 0, (gridZ - 1) * gridSpacingOffset);
                SpawnTiles(spawnPos, Quaternion.identity);
            }
        }
        if (SouthWallExit)
        {
            MakeHoles();
            Instantiate(Testers.GameTesters[4], gridOrigin + new Vector3(0, 0, roomTesterOffset) + new Vector3(0, 0, (gridZ - 1) * gridSpacingOffset), Quaternion.identity);
        }
    }

    //ceiling of room
    void SpawnCeiling()
    {
        
        CurrentGrid = new List<GameObject>();
        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                Vector3 spawnPos = new Vector3(x * gridSpacingOffset, 0, z * gridSpacingOffset) + gridOrigin + new Vector3(0, (gridY - 1) * gridSpacingOffset, 0);
                SpawnTiles(spawnPos, Quaternion.identity);
            }
        }
        if (TopExit)
        {
            MakeHoles();
            Instantiate(Testers.GameTesters[5], gridOrigin + new Vector3(0, roomTesterOffset, 0) + new Vector3(0, (gridY - 1) * gridSpacingOffset, 0), Quaternion.identity);
        }
    }
    //rooms Light
    void SpawnLights()
    {
        Light lightComp = Instantiate(GameLight, new Vector3(gridX / 2 * gridSpacingOffset, gridY / 2 * gridSpacingOffset, gridZ / 2 * gridSpacingOffset) + gridOrigin, Quaternion.identity).AddComponent<Light>();//GameLight.AddComponent<Light>();
        lightComp.color = Color.yellow;
    }

    void MakeHoles() 
    {
       
        int zLayer = 0;
       for (int x = 0; x < gridX; x++)
       {
           for (int z = 0; z < gridZ; z++)
           {
               if(z >= DoorWidthStart && z <= DoorWidthEnd && x >= DoorHeightStart && x <= DoorHeightEnd)
               {
                   Destroy(CurrentGrid[zLayer]);
                   Instantiate(Hole, CurrentGrid[zLayer].transform.position, Quaternion.identity);
               }
               zLayer++;
           }
       }
        CurrentGrid.Clear();
    }

    protected virtual void OnRoomSpawn()
    {

    }
}
