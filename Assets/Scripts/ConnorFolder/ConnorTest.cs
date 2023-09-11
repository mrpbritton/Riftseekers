using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnorTest : MonoBehaviour {

    private void Awake() {
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
        Inventory.setActiveItem(1, 0);
        Inventory.setActiveItem(3, 1);
        Inventory.setActiveItem(2, 2);
    }
}
