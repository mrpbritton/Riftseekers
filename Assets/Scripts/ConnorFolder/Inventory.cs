using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory {
    static int maxItemCount = 10;  //  this number is the max number of items the player can hold
    public static Bag itemBag = new Bag(maxItemCount);

    static string bagTag = "BagTag";
    static string activeItemTag(int ind) {
        //  checks if ind is usable
        if(ind > 3 || ind < 0)
            Debug.LogError("Used Invalid Active Item Slot");
        return "ActiveItemTag" + ind.ToString();
    }

    //  saves the indexes of active items
    public static void setActiveItem(int indexInBag, int usedActiveSlot) {
        SaveData.setInt(activeItemTag(usedActiveSlot), indexInBag);
    }
    public static ConItem getActiveItem(int slotInd) {
        int ind = SaveData.getInt(activeItemTag(slotInd), -1);
        return ind == -1 ? null : getItem(ind);
    }
    
    //  Item saving things
    public static void saveInventory() {
        var d = JsonUtility.ToJson(itemBag);
        SaveData.setString(bagTag, d);
    }
    public static void loadInventory() {
        var d = SaveData.getString(bagTag);
        itemBag = string.IsNullOrEmpty(d) ? new Bag(maxItemCount) : JsonUtility.FromJson<Bag>(d);
    }

    public static void addItem(ConItem i) {
        itemBag.items.Add(new ItemSaveData(i));
    }
    public static void removeItem(int i) {
        if(i < itemBag.items.Count)
            itemBag.items.RemoveAt(i);
    }

    public static ConItem getItem(int i) {
        if(i < itemBag.items.Count)
            return itemBag.items[i].toItem();
        return null;
    }
}

[System.Serializable]
public class Bag {
    public int maxCount;
    public List<ItemSaveData> items;

    public Bag(int max, List<ConItem> i = null) {
        maxCount = max;
        items = new List<ItemSaveData>();
        if(i != null) {
            foreach(var j in i) {
                items.Add(new ItemSaveData(j));
            }
        }
    }
}

[System.Serializable]
public class ItemSaveData {
    public string title;
    public string description;
    public int value;

    public ItemSaveData(ConItem i) {
        title = i.title;
        description = i.description;
        value = i.value;
    }
    public ConItem toItem() {
        var temp = new ConItem();
        temp.title = title;
        temp.description = description;
        temp.value = value;
        return temp;
    }
}