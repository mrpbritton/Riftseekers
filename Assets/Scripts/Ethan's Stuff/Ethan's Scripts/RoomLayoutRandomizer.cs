using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLayoutRandomizer : MonoBehaviour
{
    int[,] tiles = new int[14, 14];
    void Start()
    {
        
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
        tiles[Random.Range(0, 14), Random.Range(0, 14)] = 1;//This is the starting room


    }
}
