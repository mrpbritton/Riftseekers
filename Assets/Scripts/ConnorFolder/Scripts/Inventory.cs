using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Inventory {
    static int maxItemCount = 9;  //  this number is the max number of items the player can hold
    static Bag itemBag = new Bag(250, maxItemCount);

    static string bagTag = "BagTag";

    //  Item saving things
    public static void saveInventory() {
        var d = JsonUtility.ToJson(itemBag);
        SaveData.setString(bagTag, d);
    }
    public static void loadInventory() {
        var d = SaveData.getString(bagTag);
        itemBag = string.IsNullOrEmpty(d) ? new Bag(250, maxItemCount) : JsonUtility.FromJson<Bag>(d);
    }

    public static void addItem(ConItem i) {
        itemBag.items.Add(new ItemSaveData(i));

        saveInventory();
    }
    public static void overrideItem(int ind, ConItem i) {
        itemBag.items[ind] = new ItemSaveData(i);

        saveInventory();
    }
    public static void removeItem(int i) {
        if(i < itemBag.items.Count)
            itemBag.items.RemoveAt(i);

        saveInventory();
    }
    public static void clear() {
        itemBag.items.Clear();

        saveInventory();
    }

    public static void changeMoney(int chng) {
        itemBag.money += chng;

        saveInventory();
    }
    public static int getMoney() {
        return itemBag.money;
    }

    public static ConItem getItem(int index, AugmentLibrary il) {
        if(index < itemBag.items.Count)
            return getItem(itemBag.items[index].toItem(il), il);
        return null;
    }
    public static ConItem getItem(ConItem i, AugmentLibrary il) {
        foreach(var j in il.getAllItems()) {
            if(j.title == i.title)
                return j;
        }
        return null;
    }
    public static List<ConItem> getItems(AugmentLibrary il) {
        var temp = new List<ConItem>();
        foreach(var i in itemBag.items)
            temp.Add(i.toItem(il));
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

    public static int itemCount() {
        return itemBag.items.Count;
    }

    public static int getRandLoreIndex() {
        if(itemBag.unseenLore.Count == 0) return -1;
        return itemBag.unseenLore[Random.Range(0, itemBag.unseenLore.Count)];
    }
    public static void removeLoreIndex(int index) {
        itemBag.unseenLore.Remove(index);
    }

    public static int getWaveIndex() {
        return itemBag.waveIndex;
    }
    public static void setWaveIndex(int index) {
        itemBag.waveIndex = index;
    }
}

[System.Serializable]
public class Bag {
    public int maxCount;
    public List<ItemSaveData> items;
    public int money;
    public List<int> unseenLore = new List<int>();
    public int waveIndex;

    public Bag(int m, int max, List<ConItem> i = null) {
        maxCount = max;
        money = m;
        items = new List<ItemSaveData>();
        if(i != null) {
            foreach(var j in i) {
                items.Add(new ItemSaveData(j));
            }
        }

        for(int j = 0; j < AugmentLibrary.I.getLoreCount(); j++) {
            unseenLore.Add(j);
        }

        waveIndex = 0;
    }
}

[System.Serializable]
public class ItemSaveData {
    public string title;
    public string description;
    public int value;
    public Sprite image;
    public Attack.AttackType overrideAbil;

    public ItemSaveData(ConItem i) {
        title = i.title;
        description = i.description;
        value = i.value;
        image = i.image;
        overrideAbil = i.overrideAbil;
    }
    public ConItem toItem(AugmentLibrary il) {
        foreach(var i in il.getAllItems()) {
            if(i.title == title)
                return i;
        }
        return null;
    }
} 