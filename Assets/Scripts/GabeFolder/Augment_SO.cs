using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Augment_SO : ScriptableObject
{
    public string augName;
    public string description;
    public Sprite sprite;
    public CharStats statOne;
    [Tooltip("Percentage modifier")]
    [Range(0,1)]
    public float oneMod; 
    public CharStats statTwo;
    [Tooltip("Percentage modifier")]
    [Range(0, 1)] 
    public float twoMod; 
    public CharStats statThree;
    [Tooltip("Percentage modifier")]
    [Range(0, 1)] 
    public float threeMod;
}
