using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory {
    static int maxItemCount = 10;  //  this number is the max number of items the player can hold
    public static Bag itemBag = new Bag(maxItemCount);

    static string bagTag = "BagTag";

    public static void saveInventory() {
        var d = JsonUtility.ToJson(itemBag);
        SaveData.setString(bagTag, d);
    }
    public static void loadInventory() {
        var d = SaveData.getString(bagTag);
        itemBag = string.IsNullOrEmpty(d) ? new Bag(maxItemCount) : JsonUtility.FromJson<Bag>(d);
    }

    public static void addItem(Item i) {
        itemBag.items.Add(i);
    }
    public static void removeItem(Item i) {
        if(itemBag.items.Contains(i))
            itemBag.items.Remove(i);
    }
    public static void removeItem(int i) {
        if(i < itemBag.items.Count)
            itemBag.items.RemoveAt(i);
    }

    public static Item getItem(int i) {
        Debug.Log(itemBag.items.Count);
        if(i < itemBag.items.Count)
            return itemBag.items[i];
        return null;
    }
}

[System.Serializable]
public class Bag {
    public int maxCount;
    public List<Item> items;

    public Bag(int max, List<Item> i = null) {
        maxCount = max;
        items = i == null ? new List<Item>() : i;
    }
}