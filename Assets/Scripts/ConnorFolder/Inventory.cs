using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory {
    public static Bag itemBag = new Bag(10);  //  this number is the max number of items the player can hold

    static string bagTag = "BagTag";

    public static void saveInventory() {
        var d = JsonUtility.ToJson(itemBag);
        SaveData.setString(bagTag, d);
    }
    public static Bag loadInventory() {
        var d = SaveData.getString(bagTag);
        if(string.IsNullOrEmpty(d)) return null;
        return JsonUtility.FromJson<Bag>(d);
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
}

[System.Serializable]
public class Bag {
    public int maxCount;
    public List<Item> items;

    public Bag(int max, List<Item> i = null) {
        maxCount = max;
        items = i == null ? new List<Item> () : i;
    }
}