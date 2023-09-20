using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeRooms : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int roomCount;

    void Start()
    {
        RandomizeRoomOrder();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RandomizeRoomOrder()//takes the randomized array and puts the rooms from the array of rooms in that order
    {
        RoomSaver.saveroom(RandomizeList());
    }
    private int[] RandomizeList()//Randomises an array of values from 1 to 10
    {
        int[] rooms = new int[roomCount];
        int number;
        for (int i = 0; i < roomCount; i++)
        {
            bool yes = false;
            do
            {
                yes = false;
                number = Random.Range(1, roomCount + 1);
                foreach (int thing in rooms)
                {
                    if (thing == number)
                    {
                        yes = true;
                    }
                }
            } while (yes);
            rooms[i] = number;
        }
        return rooms;
    }
}


[System.Serializable]
public class RoomOrder{
    public int[] roomOrder;
}
