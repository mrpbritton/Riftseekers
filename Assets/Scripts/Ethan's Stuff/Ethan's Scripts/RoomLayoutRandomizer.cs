using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomLayoutRandomizer : MonoBehaviour
{
    [Tooltip("The maximum span the tiles can go")]
    public int levelSize;
    [Tooltip("The amount of tiles generated. Setting to higher than levelSize squared will break the program")]
    public int tileCount;
    [Tooltip("The concentration that Obstacles will spawn")]
    public int barrelDensity;
    private bool generating;
    private bool place;
    [SerializeField]
    private GameObject tile, wall;
    private int chance;
    public List<GameObject> floorTiles, walls, shortWalls, blockades, enemyTiles;
    public NavMeshBuildSettings buildSettings;
    public List<NavMeshBuildSource> buildSources = new List<NavMeshBuildSource>();
    void Start()
    {
        Randomize(levelSize, tileCount);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Randomize(int listSize, int roomCount)
    {
        int[,] tiles = new int[listSize, listSize];
        int[,] altTiles = new int[listSize, listSize];
        /*Codes for 1st iteration of randomizer:
         * 0: unassigned
         * 1: starting room
         * 2: single room
         * 3: big room
         * 5: Exit room
         *-1: empty room
         */
        List<int> roomX = new List<int>();
        List<int> roomY = new List<int>();
        List<int> roomAdj = new List<int>();
        //int levelModifier = 20;
        int failsafe = 0;
        int room;//The room the new room will be placed next to
        int randomvar;//Which direction the new room will be placed
        int num; //holds the roomAdj for each number to check which rooms a room is adjacent to
        bool skip;//used if the room decides not to place
        generating = true;
        roomX.Add(listSize / 2);
        roomY.Add(listSize / 2);
        roomAdj.Add(0);
        tiles[roomX[0], roomY[0]] = 1;//This is the starting room
        if (roomY[0] == 0)
        {
            roomAdj[0] += 8;
        }
        if (roomX[0] == listSize - 1)
        {
            roomAdj[0] += 4;
        }
        if (roomY[0] == listSize - 1)
        {
            roomAdj[0] += 2;
        }
        if (roomX[0] == 0)
        {
            roomAdj[0] += 1;
        }
        for (int i = 0; i < roomCount - 1; i++)
        {
            //Place a room adjacent to an existing room
            do
            {
                room = Random.Range(0, roomX.Count);//pick the room that the new room will now be adjacent to
            } while (roomAdj[room] >= 15 || !WillPlaceRoom(roomX[room], roomY[room], tiles, listSize));
            place = true;
            failsafe = 0;
            skip = false;
            do
            {
                failsafe += 1;
                randomvar = Random.Range(0, 4);
                num = roomAdj[room];
                if (randomvar == 0 && roomY[room] != 0 && num < 8)//up
                {
                    if (tiles[roomX[room], roomY[room] - 1] == 0)//if the room can be placed
                    {
                        if (WillPlaceRoom(roomX[room], roomY[room] - 1, tiles, listSize))
                        {
                            roomX.Add(roomX[room]);
                            roomY.Add(roomY[room] - 1);
                            //Debug.Log("Up: [" + roomX[room] + "," + roomY[room] + "]");
                            roomAdj.Add(0);
                            tiles[roomX[room], roomY[room] - 1] += 1;
                            place = false;
                        }
                        else
                        {
                            place = false;
                            skip = true;
                        }
                    }
                }
                else if (num >= 8)
                {
                    num -= 8;
                }
                if (randomvar == 1 && roomX[room] != listSize - 1 && num < 4)//right
                {
                    if (tiles[roomX[room] + 1, roomY[room]] == 0)//if the room can be placed
                    {
                        if (WillPlaceRoom(roomX[room] + 1, roomY[room], tiles, listSize))
                        {
                            roomX.Add(roomX[room] + 1);
                            roomY.Add(roomY[room]);
                            //Debug.Log("Right: [" + roomX[room] + "," + roomY[room] + "]");
                            roomAdj.Add(0);
                            tiles[roomX[room] + 1, roomY[room]] += 1;
                            place = false;
                        }
                        else
                        {
                            place = false;
                            skip = true;
                        }
                    }
                }
                else if (num >= 4)
                {
                    num -= 4;
                }
                if (randomvar == 2 && roomY[room] != listSize - 1 && num < 2)//down
                {
                    if (tiles[roomX[room], roomY[room] + 1] == 0)//if the room can be placed
                    {
                        if (WillPlaceRoom(roomX[room], roomY[room] + 1, tiles, listSize))
                        {
                            roomX.Add(roomX[room]);
                            roomY.Add(roomY[room] + 1);
                            //Debug.Log("Down: [" + roomX[room] + "," + roomY[room] + "]");
                            roomAdj.Add(0);
                            tiles[roomX[room], roomY[room] + 1] += 1;
                            //Debug.Log(roomAdj[roomAdj.Count-1]);
                            place = false;
                        }
                        else
                        {
                            place = false;
                            skip = true;
                        }
                    }
                }
                else if (num >= 2)
                {
                    num -= 2;
                }
                if (randomvar == 3 && roomX[room] != 0 && num < 1)//left
                {
                    if (tiles[roomX[room] - 1, roomY[room]] == 0)//if the room can be placed
                    {
                        if (WillPlaceRoom(roomX[room] - 1, roomY[room], tiles, listSize))
                        {
                            roomX.Add(roomX[room] - 1);
                            roomY.Add(roomY[room]);
                            //Debug.Log("Left: [" + roomX[room] + "," + roomY[room] + "]");
                            roomAdj.Add(0);
                            num = roomAdj[roomX.Count - 1];
                            tiles[roomX[room] - 1, roomY[room]] += 1;
                            place = false;
                        }
                        else
                        {
                            place = false;
                            skip = true;
                        }
                    }
                }
            } while (place && failsafe <= 50);
            if (!skip)
            {
                if (roomY[^1] > 0)
                {
                    if (tiles[roomX[^1], roomY[^1] - 1] != 0)//up room
                    {
                        for (int j = 0; j < roomX.Count; j++)
                        {
                            if (roomX[j] == roomX[^1] && roomY[j] == roomY[^1] - 1)
                            {
                                roomAdj[j] += 2;//Is the room on top, so is showing that it is adjacent to down room
                                roomAdj[^1] += 8;
                            }
                        }
                    }
                }
                else
                {
                    roomAdj[^1] += 8;
                }
                if (roomX[^1] < listSize - 1)
                {
                    if (tiles[roomX[^1] + 1, roomY[^1]] != 0)//right room
                    {
                        for (int j = 0; j < roomX.Count; j++)
                        {
                            if (roomX[j] == roomX[^1] + 1 && roomY[j] == roomY[^1])
                            {
                                roomAdj[j] += 1;//Is the room on top, so is showing that it is adjacent to down room
                                roomAdj[^1] += 4;
                            }
                        }
                    }
                }
                else
                {
                    roomAdj[^1] += 4;
                }
                if (roomY[^1] < listSize - 1)
                {
                    if (tiles[roomX[^1], roomY[^1] + 1] != 0)//down room
                    {
                        for (int j = 0; j < roomX.Count; j++)
                        {
                            if (roomX[j] == roomX[^1] && roomY[j] == roomY[^1] + 1)
                            {
                                roomAdj[j] += 8;//Is the room on top, so is showing that it is adjacent to down room
                                roomAdj[^1] += 2;
                            }
                        }
                    }
                }
                else
                {
                    roomAdj[^1] += 2;
                }
                if (roomX[^1] > 0)
                {
                    if (tiles[roomX[^1] - 1, roomY[^1]] != 0)//up room
                    {
                        for (int j = 0; j < roomX.Count; j++)
                        {
                            if (roomX[j] == roomX[^1] - 1 && roomY[j] == roomY[^1])
                            {
                                roomAdj[j] += 4;//Is the room on top, so is showing that it is adjacent to down room
                                roomAdj[^1] += 1;
                            }
                        }
                    }
                }
                else
                {
                    roomAdj[^1] += 1;
                }
            }
            else
            {
                i--;
            }
            if (failsafe > 50)
            {
                Debug.Log("Failsafe Triggered");
                Debug.Log("RoomAdj: [" + roomX[room] + "," + roomY[room] + "]");
            }
        }
        for(int i = 0; i < levelSize; i++)
        {
            for(int j = 0; j < levelSize; j++)
            {
                altTiles[i, j] = tiles[i, j];
            }
        }
        for (int i = 0; i < listSize; i++)//puts the rooms in place physically
        {
            for (int j = 0; j < listSize; j++)
            {
                if (tiles[i, j] != 0)
                {
                    //Debug.Log(tiles[i, j]);
                    GameObject thingTile = PlaceTile(altTiles,i,j);
                    //GameObject thingTile = Instantiate(tile, new Vector3(4.25f * (i/Mathf.Sqrt(2) - j/Mathf.Sqrt(2)) , 0, 4.25f * (i/Mathf.Sqrt(2) + j/Mathf.Sqrt(2))), Quaternion.identity);
                    buildSources.Add(new NavMeshBuildSource() { shape = NavMeshBuildSourceShape.Box, size = new Vector3(4.25f, .01f, 4.25f),transform = Matrix4x4.TRS(thingTile.transform.position + new Vector3(0,0,3),Quaternion.Euler(0,45,0), Vector3.one)});

                    //thingTile.transform.eulerAngles = new Vector3(0, 45, 0);
                    if (j == listSize - 1 || tiles[i, j + 1] == 0)
                    {
                        if (j < listSize - 2 && tiles[i, j + 2] != 0 || (j < listSize - 1 && i < listSize - 1 && tiles[i + 1, j + 1] != 0))
                        {
                            PlaceWall(thingTile, wall, true, 0, 6, -90);
                        }
                        else
                        {
                            PlaceWall(thingTile, wall, false, 0, 6, -90);


                        }
                    }
                    if (j == 0 || tiles[i, j - 1] == 0)//short wall, bottom right
                    {
                        GameObject thing = Instantiate(wall, new Vector3(thingTile.transform.position.x, .5f, thingTile.transform.position.z), Quaternion.identity);
                        thing.transform.eulerAngles = new Vector3(0, 90, 0);

                    }
                    if (i == listSize - 1 || tiles[i + 1, j] == 0)
                    {
                        if ((i < 12 && tiles[i + 2, j] != 0) || (i < listSize - 1 && j < listSize - 1 && tiles[i + 1, j + 1] != 0))
                        {
                            GameObject thing = Instantiate(wall, new Vector3(thingTile.transform.position.x + 3, .5f, thingTile.transform.position.z + 3), Quaternion.identity);
                            //thing.transform.eulerAngles = new Vector3(0, -90, 0);
                        }
                        else
                        {
                            GameObject thing = Instantiate(wall, new Vector3(thingTile.transform.position.x + 3, 2, thingTile.transform.position.z + 3), Quaternion.identity);
                            //thing.transform.eulerAngles = new Vector3(0, -90, 0);
                        }
                    }
                    if (i == 0 || tiles[i - 1, j] == 0)//short wall, bottom left
                    {
                        GameObject thing = Instantiate(wall, new Vector3(thingTile.transform.position.x - 3, .5f, thingTile.transform.position.z + 3), Quaternion.identity);
                        thing.transform.eulerAngles = new Vector3(0, 180, 0);
                    }
                }
            }
        }
        NavMeshData built = NavMeshBuilder.BuildNavMeshData(buildSettings, buildSources, new Bounds(new Vector3(0, 0, levelSize * 4.25f / Mathf.Sqrt(2)), new Vector3(levelSize * 5, 10, levelSize * 5)), Vector3.zero, Quaternion.identity);
        NavMesh.AddNavMeshData(built);
    }
    public bool WillPlaceRoom(int X, int Y, int[,] tiles, int levelSize)
    {
        int chance = 0;//Figure out how many rooms the selected room is bordering
        //Debug.Log("Pos: [" + X + "," + Y + "]");
        if (X != 0 && tiles[X - 1, Y] >= 1)
        {
            chance += 1;
        }
        if (Y != 0 && tiles[X, Y - 1] >= 1)
        {
            chance += 1;
        }
        if (X != levelSize - 1 && tiles[X + 1, Y] >= 1)
        {
            chance += 1;
        }
        if (Y != levelSize - 1 && tiles[X, Y + 1] >= 1)
        {
            chance += 1;
        }
        if (chance <= 1)//If bordering one or two rooms, then just place it
        {

            //Debug.Log("Adjacencies: " + chance + " placed");
            return true;
        }
        else if (chance == 2)//If bordering 2 rooms, place it half of the time
        {
            if (Random.Range(0, 1) == 0)
            {
                //Debug.Log("Adjacencies: " + chance + " placed");
                return true;
            }
            else
            {
                //Debug.Log("Adjacencies: " + chance + " failed");
                return false;
            }
        }
        else// If bordering 3 rooms, have a 1 in 4 chance of placing it
        {
            if (Random.Range(0, 20) == 0)
            {
                //Debug.Log("Adjacencies: " + chance + " placed");
                return true;
            }
            else
            {
                //Debug.Log("Adjacencies: " + chance + " failed");
                return false;
            }
        }
    }
    public bool WillBlockade(int X, int Y, int[,] grid, int levelSize)
    {
        int[,] tileGrid = new int[levelSize,levelSize];
        for(int i = 0; i < levelSize; i++)
        {
            for(int j = 0; j < levelSize; j++)
            {
                tileGrid[i, j] = grid[i, j];
            }
        }
        int tileCount = 0;
        tileGrid[X, Y] = 0;//The tile we want to remove
        List<int[]> tileQueue = new List<int[]>();
        List<int[]> tilesChecked = new List<int[]>();

        for (int i = 0; i < levelSize; i++)//How many tiles are in the level
        {
            for(int j = 0; j < levelSize; j++)
            {
                if(tileGrid[i,j] != 0)
                {
                    if (tileQueue.Count == 0)
                        tileQueue.Add(new int[] { i, j });//put the first available tile into the Queue
                    tileCount++;
                }
            }
        }
        int debug = 0;
        while(tileQueue.Count != 0)//scan the grid from the first square
        {
            int[] tileCheck;
            //For each loop
            /*if(tileQueue[0][0] != 0 && tiles[tileQueue[0][0] - 1, tileQueue[0][1]] != 0 && !tileQueue.Contains(new int[] { tileQueue[0][0] - 1, tileQueue[0][1] }) && !tilesChecked.Contains(new int[] { tileQueue[0][0] - 1, tileQueue[0][1] }))//If not on the edge and there is a tile in the -i direction, and the list is not in the queue or checked tiles
            {
                tileQueue.Add(new int[] { tileQueue[0][0] - 1, tileQueue[0][1] });
            }*/
            if(tileQueue[0][0] > 0)
            {
                //Debug.Log("[" + tileQueue[0][0] + "," + tileQueue[0][1] + "]");
                tileCheck = new int[] { tileQueue[0][0] - 1, tileQueue[0][1] };
                //Debug.Log(tileCheck[0] + "," + tileCheck[1]);
                if (tileGrid[tileCheck[0], tileCheck[1]] != 0)
                {
                    //Debug.Log("Check 2");

                    if(!IsInList(tileQueue,tileCheck))
                    {
                        //Debug.Log("Check 3");
                        if(!IsInList(tilesChecked,tileCheck))
                        {
                            //Debug.Log("Check 4");
                            tileQueue.Add(new int[] { tileQueue[0][0] - 1, tileQueue[0][1] });
                        }
                    }
                }
            }
            if (tileQueue[0][1] != 0)
            {
                tileCheck = new int[] { tileQueue[0][0], tileQueue[0][1] - 1 };
                if (tileGrid[tileCheck[0], tileCheck[1]] != 0 && !IsInList(tileQueue, tileCheck) && !IsInList(tilesChecked, tileCheck))//If not on the edge and there is a tile in the -j direction, and the list is not in the queue or checked tiles
                {
                    tileQueue.Add(new int[] { tileQueue[0][0], tileQueue[0][1] - 1 });
                }
            }
            if (tileQueue[0][0] != levelSize - 1)
            {
                tileCheck = new int[] { tileQueue[0][0] + 1, tileQueue[0][1] };
                if (tileGrid[tileCheck[0], tileCheck[1]] != 0 && !IsInList(tileQueue, tileCheck) && !IsInList(tilesChecked, tileCheck))//If not on the edge and there is a tile in the +i direction, and the list is not in the queue or checked tiles
                {
                    tileQueue.Add(new int[] { tileQueue[0][0] + 1, tileQueue[0][1] });
                }
            }
            if (tileQueue[0][1] != levelSize - 1)//If not on the edge and there is a tile in the +j direction, and the list is not in the queue or checked tiles
            {
                tileCheck = new int[] { tileQueue[0][0] , tileQueue[0][1] + 1 };
                if (tileGrid[tileCheck[0], tileCheck[1]] != 0 && !IsInList(tileQueue, tileCheck) && !IsInList(tilesChecked, tileCheck))
                {
                    tileQueue.Add(new int[] { tileQueue[0][0], tileQueue[0][1] + 1 });
                }
            }
            tilesChecked.Add(tileQueue[0]);
            //Debug.Log("[" + tilesChecked[^1][0] + "," + tilesChecked[^1][1] + "]");
            tileQueue.RemoveAt(0);
            /*foreach(int[] item in tilesChecked)
            {
                Debug.Log("{" + item[0] + "," + item[1] + "}");
            }*/
            debug++;
            if(debug > 100)
            {
                Debug.Log("Error");
                return false;
            }
        }
        if(tilesChecked.Count == tileCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void PlaceWall(GameObject tile,GameObject wall, bool shorter, int left, int right, int spin)
    {
        float highPos = 2;
        if (shorter)
        {
            highPos = .5f;
        }
        GameObject thing = Instantiate(wall, new Vector3(tile.transform.position.x + left, highPos, tile.transform.position.z + right), Quaternion.identity);
        thing.transform.eulerAngles = new Vector3(0, spin, 0);
        return;
    }
    public GameObject PlaceTile(int[,] grid,int X, int Y)//Will place a tile at the desired location, with a chance of being a blockade or an enemy tile
    {
        if (WillBlockade(X, Y, grid, levelSize) && Adjacencies(grid,levelSize,X,Y) != 1 && !(X == levelSize / 2 && Y == levelSize / 2))
        {
            if(Random.Range(0,4) == 0)
            {
                grid[X, Y] = 0;
                if (Random.Range(0, 2) == 0)
                {
                    return Instantiate(blockades[0], new Vector3(4.25f * (X / Mathf.Sqrt(2) - Y / Mathf.Sqrt(2)), 0, 4.25f * (X / Mathf.Sqrt(2) + Y / Mathf.Sqrt(2))), Quaternion.identity);
                }
                else
                {
                    return Instantiate(enemyTiles[0], new Vector3(4.25f * (X / Mathf.Sqrt(2) - Y / Mathf.Sqrt(2)), 0, 4.25f * (X / Mathf.Sqrt(2) + Y / Mathf.Sqrt(2))), Quaternion.identity);

                }
            }
            else
            {
                return Instantiate(floorTiles[0], new Vector3(4.25f * (X / Mathf.Sqrt(2) - Y / Mathf.Sqrt(2)), 0, 4.25f * (X / Mathf.Sqrt(2) + Y / Mathf.Sqrt(2))), Quaternion.identity);
            }
        }
        else
        {
            return Instantiate(floorTiles[0], new Vector3(4.25f * (X / Mathf.Sqrt(2) - Y / Mathf.Sqrt(2)), 0, 4.25f * (X / Mathf.Sqrt(2) + Y / Mathf.Sqrt(2))), Quaternion.identity);
        }
    }
    public bool IsInList(List<int[]> checker, int[] target)
    {
        foreach(int[] tile in checker)
        {
            if(tile[0] == target[0] && tile[1] == target[1])
            {
                return true;
            }
        }
        return false;
    }
    public int Adjacencies(int[,] tiles, int levelSize, int X, int Y)
    {
        int adjacent = 0;
        if (X != 0 && tiles[X - 1, Y] != 0)
        {
            adjacent += 1;
        }
        if (X != levelSize - 1 && tiles[X + 1, Y] != 0)
        {
            adjacent += 1;
        }
        if (Y != levelSize - 1 && tiles[X, Y + 1] != 0)
        {
            adjacent += 1;
        }
        if (Y != 0 && tiles[X, Y - 1] != 0)
        {
            adjacent += 1;
        }
        return adjacent;
    }
}
