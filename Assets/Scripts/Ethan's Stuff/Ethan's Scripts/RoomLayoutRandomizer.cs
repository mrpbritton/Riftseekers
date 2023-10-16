using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLayoutRandomizer : MonoBehaviour
{
    int[,] tiles = new int[14, 14];
    private bool generating;
    private bool place;
    [SerializeField]
    private GameObject tile, wall;
    private int chance;
    public List<GameObject> floorTiles, walls, blockades;

    public struct roomDirection
    {
        public bool up;
        public bool down;
        public bool right;
        public bool left;
    }
    void Start()
    {
        Randomize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Randomize()
    {

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
        roomX.Add(7);
        roomY.Add(7);
        roomAdj.Add(0);
        tiles[roomX[0],roomY[0]] = 1;//This is the starting room
        if(roomY[0] == 0)
        {
            roomAdj[0] += 8;
        }
        if (roomX[0] == 13)
        {
            roomAdj[0] += 4;
        }
        if (roomY[0] == 13)
        {
            roomAdj[0] += 2;
        }
        if (roomX[0] == 0)
        {
            roomAdj[0] += 1;
        }
        for (int i = 0; i < 49; i++)
        {
            //Place a room adjacent to an existing room
            do
            {
                room = Random.Range(0, roomX.Count);//pick the room that the new room will now be adjacent to
            } while (roomAdj[room] >= 15 || !WillPlaceRoom(roomX[room], roomY[room]));
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
                        if (WillPlaceRoom(roomX[room], roomY[room] - 1))
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
                else if(num >= 8)
                {
                    num -= 8;
                }
                if (randomvar == 1 && roomX[room] != 13 && num < 4)//right
                {
                    if (tiles[roomX[room] + 1, roomY[room]] == 0)//if the room can be placed
                    {
                        if (WillPlaceRoom(roomX[room] + 1, roomY[room]))
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
                else if(num >= 4)
                {
                    num -= 4;
                }
                if (randomvar == 2 && roomY[room] != 13 && num < 2)//down
                {
                    if (tiles[roomX[room], roomY[room] + 1] == 0)//if the room can be placed
                    {
                        if (WillPlaceRoom(roomX[room], roomY[room] + 1))
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
                else if(num >= 2)
                {
                    num -= 2;
                }
                if (randomvar == 3 && roomX[room] != 0 && num < 1)//left
                {
                    if (tiles[roomX[room] - 1, roomY[room]] == 0)//if the room can be placed
                    {
                        if (WillPlaceRoom(roomX[room] - 1, roomY[room]))
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
            } while (place && failsafe <= 20);
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
                if (roomX[^1] < 13)
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
                if (roomY[^1] < 13)
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
            if (failsafe > 20)
            {
                Debug.Log("Failsafe Triggered");
                Debug.Log("RoomAdj: [" + roomX[room] + "," + roomY[room] + "]");
            }
            

            for (int n = 0; n < roomAdj.Count; n++)
            {
                //Debug.Log("[" + roomX[n] + "," + roomY[n] + "]: " + roomAdj[n]);
            }


        }
        for (int i = 0; i < 14; i++)//puts the rooms in place physically
        {
            for(int j = 0; j < 14; j++)
            {
                if(tiles[i,j] != 0)
                {
                    //Debug.Log(tiles[i, j]);
                    Instantiate(tile,new Vector3(i,0,j), Quaternion.identity);
                    if(j == 13 || tiles[i,j+1] == 0)
                    {
                        if(j < 12 && tiles[i,j+2] != 0 || (j < 13 && i < 13 && tiles[i + 1, j + 1] != 0))
                        {
                            GameObject thing = Instantiate(wall, new Vector3(i, 0.5f, j + .5f), Quaternion.identity);
                            thing.transform.eulerAngles = new Vector3(0, 90, 0);
                        }
                        else
                        {
                            GameObject thing = Instantiate(wall, new Vector3(i, 1, j + .5f), Quaternion.identity);
                            thing.transform.eulerAngles = new Vector3(0, 90, 0);
                        }
                    }
                    if (j == 0 || tiles[i, j - 1] == 0)//short wall
                    {
                        GameObject thing = Instantiate(wall, new Vector3(i, .5f, j - .5f), Quaternion.identity);
                        thing.transform.eulerAngles = new Vector3(0, 90, 0);
                    }
                    if (i == 13 ||tiles[i + 1, j] == 0)
                    {
                        if ((i < 12 && tiles[i + 2, j] != 0) || (i < 13 && j < 13 && tiles[i + 1, j + 1] != 0))
                        {
                            GameObject thing = Instantiate(wall, new Vector3(i + .5f, .5f, j), Quaternion.identity);
                        }
                        else
                        {
                            GameObject thing = Instantiate(wall, new Vector3(i + .5f, 1, j), Quaternion.identity);
                        }
                    }
                    if (i == 0 || tiles[i - 1, j] == 0)//short wall
                    {
                        GameObject thing = Instantiate(wall, new Vector3(i - .5f, .5f, j), Quaternion.identity);
                    }
                }
            }
        }
        for(int i = 0; i < roomAdj.Count; i++)
        {
            //Debug.Log("[" + roomX[i] + "," + roomY[i] + "]: " + roomAdj[i]);
        }
    }
    public bool WillPlaceRoom(int X, int Y)
    {
        int chance = 0;//Figure out how many rooms the selected room is bordering
        Debug.Log("Pos: [" + X + "," + Y + "]");
        if (X != 0 && tiles[X - 1, Y] >= 1)
        {
            chance += 1;
        }
        if (Y != 0 && tiles[X, Y - 1] >= 1)
        {
            chance += 1;
        }
        if (X != 13 && tiles[X + 1, Y] >= 1)
        {
            chance += 1;
        }
        if (Y != 13 && tiles[X, Y + 1] >= 1)
        {
            chance += 1;
        }
        if(chance <= 1)//If bordering one or two rooms, then just place it
        {
            
            Debug.Log("Adjacencies: " + chance + " placed");
            return true;
        }
        else if(chance == 2)//If bordering 2 rooms, place it half of the time
        {
            if(Random.Range(0,1) == 0)
            {
                Debug.Log("Adjacencies: " + chance + " placed");
                return true;
            }
            else
            {
                Debug.Log("Adjacencies: " + chance + " failed");
                return false;
            }
        }
        else// If bordering 3 rooms, have a 1 in 4 chance of placing it
        {
            if (Random.Range(0,20) == 0)
            {
                Debug.Log("Adjacencies: " + chance + " placed");
                return true;
            }
            else
            {
                Debug.Log("Adjacencies: " + chance + " failed");
                return false;
            }
        }
    }
}
