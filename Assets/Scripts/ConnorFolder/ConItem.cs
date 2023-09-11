using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class ConItem : ScriptableObject {
    [Tooltip("Name/title of the item")]
    public string title;
    [Tooltip("What this item does")]
    public string description;
    [Tooltip("How much this item sells for")]
    public int value;

    public ConItem(ItemSaveData saveData) {
        title = saveData.title;
        description = saveData.description;
        value = saveData.value;
    }
    public ConItem() { }
}
