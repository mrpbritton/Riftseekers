using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AttackScript
{
    None, Sword, Handgun, Shotgun, Rocket
}

[CreateAssetMenu]
[System.Serializable]
public class ConItem : ScriptableObject {
    [Tooltip("Name/title of the item")]
    public string title;
    [Tooltip("What this item does")]
    public string description;
    [Tooltip("How much this item sells for")]
    public int value;
    [Tooltip("What this item looks like")]
    public Sprite image;
    [Tooltip("What type of ability does this give the player")]
    public Attack.attackType overrideAbil = Attack.attackType.None;
    [Tooltip("What the type of attack script this will give the player, if any")]
    public AttackScript attackScript;
/*    [Tooltip("Item drop associated with this item")]
    public GameObject item;*/

    public ConItem(ItemSaveData saveData) {
        title = saveData.title;
        description = saveData.description;
        value = saveData.value;
        image = saveData.image;
        overrideAbil = saveData.overrideAbil;
    }
}
