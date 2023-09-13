using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory {
    static int maxItemCount = 9;  //  this number is the max number of items the player can hold
    static Bag itemBag = new Bag(maxItemCount);

    static string bagTag = "BagTag";
    static string thingTag = "thingTag";

    //  saves the indexes of active items
    public static void overrideActiveItem(int slot, ConItem item) {
        switch(slot) {
            case 0:
                itemBag.activeItem1 = new ItemSaveData(item);
                break;
            case 1:
                itemBag.activeItem2 = new ItemSaveData(item);
                break;
            case 2:
                itemBag.activeItem3 = new ItemSaveData(item);
                break;
            default:
                Debug.LogError("Trying to save an active item in an invalid slot");
                break;
        }
    }
    public static ConItem getActiveItem(int slotInd) {
        switch(slotInd) {
            case 0:
                return itemBag.activeItem1.toItem();
            case 1:
                return itemBag.activeItem2.toItem();
            case 2:
                return itemBag.activeItem3.toItem();
            default:
                Debug.LogError("Trying to save an active item in an invalid slot");
                return null;
        }
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
    public static void clear() {
        itemBag.items.Clear();
    }

    public static ConItem getItem(int i) {
        if(i < itemBag.items.Count)
            return itemBag.items[i].toItem();
        return null;
    }
    public static List<ConItem> getItems() {
        var temp = new List<ConItem>();
        foreach(var i in itemBag.items) 
            temp.Add(i.toItem());
        return temp;
    }
    public static int getItemIndex(ConItem item) {
        int index = 0;
        foreach(var i in itemBag.items) {
            if(i.title == item.title) {
                return index;
            }
            index++;
        }
        Debug.LogError("No index for item");
        return -1;
    }

    public static int itemCount(bool includeActives) {
        int temp = itemBag.items.Count;
        if(itemBag.activeItem1 != null)
            temp++;
        if(itemBag.activeItem2 != null)
            temp++;
        if(itemBag.activeItem3 != null)
            temp++;
        return temp;
    }

    public static void saveThing() {
        temp t = new temp();
        var d = JsonUtility.ToJson(t);
        SaveData.setString(thingTag, d);

        d = SaveData.getString(thingTag);
        var th = JsonUtility.FromJson<temp>(d);
    }
}

[System.Serializable]
public class Bag {
    public int maxCount;
    public List<ItemSaveData> items;

    public ItemSaveData activeItem1 = null, activeItem2 = null, activeItem3 = null;

    public Bag(int max, List<ConItem> i = null, ItemSaveData active1 = null, ItemSaveData active2 = null, ItemSaveData active3 = null) {
        maxCount = max;
        items = new List<ItemSaveData>();
        if(i != null) {
            foreach(var j in i) {
                items.Add(new ItemSaveData(j));
            }
        }
        activeItem1 = active1;
        activeItem2 = active2;
        activeItem3 = active3;
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

[System.Serializable]
public class temp {
    public int[] value;
}