using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeRooms : MonoBehaviour
{
    // Start is called before the first frame update
    private int[] test = new int[10];
    [SerializeField] private GameObject[] rooms = new GameObject[10];
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RandomizeRoomOrder()//takes the randomized array and puts the rooms from the array of rooms in that order
    {
        int[] order = new int[10];
        order = RandomizeList();
        for(int i = 0; i < 10; i++)
        {
            Instantiate(rooms[order[i] - 1], new Vector3(i * 20, 0, i * 16),Quaternion.identity);
        }
    }
    private int[] RandomizeList()//Randomises an array of values from 1 to 10
    {
        int[] rooms = new int[10];
        int number;
        for(int i = 0; i < 10; i++)
        {
            bool yes = false;
            do
            {
                yes = false;
                number = Random.Range(1, 11);
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
