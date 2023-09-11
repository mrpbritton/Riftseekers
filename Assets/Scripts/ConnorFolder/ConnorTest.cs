using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnorTest : MonoBehaviour {

    private void Awake() {
        //setsThings();
        seesThings();
    }

    void save() {
        foreach(var i in FindObjectOfType<ItemLibrary>().getItems())
            Inventory.addItem(i);
        Inventory.saveInventory();
        Debug.Log("saved");
    }

    void load() {
        Inventory.loadInventory();
        Debug.Log(Inventory.getItem(0).title);
        Debug.Log(Inventory.getItem(0).description);
        Debug.Log(Inventory.getItem(0).value);
    }

    void setsThings() {
        SaveData.wipe();
        Inventory.overrideActiveItem(0, FindObjectOfType<ItemLibrary>().getItem(1));
        Inventory.overrideActiveItem(1, FindObjectOfType<ItemLibrary>().getItem(2));
        Inventory.overrideActiveItem(2, FindObjectOfType<ItemLibrary>().getItem(0));
        Inventory.saveInventory();
    }

    void seesThings() {
        Inventory.loadInventory();
        Debug.Log(Inventory.getActiveItem(0).title);
        Debug.Log(Inventory.getActiveItem(1).title);
        Debug.Log(Inventory.getActiveItem(2).title);
    }
}
