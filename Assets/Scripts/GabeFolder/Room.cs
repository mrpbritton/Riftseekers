using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField, Tooltip("How many hazards there are in the room"), Range(1,5)]
    private int hazards;
    [SerializeField, Tooltip("How many chests there are in the room"), Range(0,1)]
    private int chests;
}
