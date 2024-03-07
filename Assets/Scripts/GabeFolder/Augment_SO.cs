using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Augment_SO : ScriptableObject
{
    public enum augmentType {
        None, SpeedBuff, DashChargeBuff, DamageBuff
    }
    public augmentType type;
    public string title;
    public string description;
    public Sprite sprite;
    public List<StatAugment> mods = new List<StatAugment>();
}

[System.Serializable]
public class StatAugment {
    public CharStats stat;
    [Tooltip("Percentage modifier")]
    [Range(0, 3)]
    public float mod;
}