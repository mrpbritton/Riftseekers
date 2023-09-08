using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnorTest : MonoBehaviour {

    private void Awake() {
        /*
        Item i = new Item();
        i.title = "This is my item";
        i.description = "Descriptor";
        i.value = 100;
        Inventory.addItem(i);
        Inventory.saveInventory();
        */
        Inventory.loadInventory();
        Debug.Log(Inventory.getItem(0));
    }
}
