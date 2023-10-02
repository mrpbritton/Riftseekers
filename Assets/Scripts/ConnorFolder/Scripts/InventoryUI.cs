using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    [SerializeField] List<Image> activeSlots;
    [SerializeField] List<Image> inactiveSlots;
    [SerializeField] Sprite emptySlotSprite;
    [SerializeField] ConItem tester, tester2;

    ItemLibrary il;

    int curIndex = -1;
    int actIndex = 0;

    private void Awake() {
        il = FindObjectOfType<ItemLibrary>();
        Inventory.loadInventory();
        //funny();
        show();
        setActIndex(0);
    }

    void funny() {
        Inventory.addItem(tester);
        Inventory.addItem(tester2);
        Inventory.addItem(tester2);
        Inventory.saveInventory();
    }

    public void show() {
        //  active items
        for(int i = 0; i < activeSlots.Count; i++) {
            bool b = Inventory.getActiveItem(i, il) != null;
            activeSlots[i].sprite = b ? Inventory.getActiveItem(i, il).image : emptySlotSprite;
        }

        //  inventory items
        int invCount = Inventory.getItems(il).Count;
        for(int i = 0; i < inactiveSlots.Count; i++) {
            inactiveSlots[i].sprite = i < invCount ? Inventory.getItems(il)[i].image : emptySlotSprite;
        }
    }

    public void setCurIndex(int ind) {
        curIndex = ind;
        ConItem temp = Inventory.getActiveItem(actIndex, il);   //  saves the active item that's being replaced

        //  overrides the active item if there is a valid overrider
        if(Inventory.getItems(il).Count > curIndex) 
            Inventory.overrideActiveItem(actIndex, Inventory.getItems(il)[curIndex]);
        //  otherwise, just remove the active item
        else 
            Inventory.removeActiveItem(actIndex);

        Inventory.removeItem(curIndex); //  removes the inventory item because it's not needed anymore
        //  adds the saved active item into the inventory if it's valid
        if(temp != null) 
            Inventory.addItem(temp);
        Inventory.saveInventory();
        show();
    }
    public void setActIndex(int ind) {
        actIndex = ind;
        switch(ind) {
            case 0:
                activeSlots[0].GetComponent<Image>().color = Color.white;
                activeSlots[1].GetComponent<Image>().color = Color.grey;
                activeSlots[2].GetComponent<Image>().color = Color.grey;
                break;
            case 1:
                activeSlots[0].GetComponent<Image>().color = Color.grey;
                activeSlots[1].GetComponent<Image>().color = Color.white;
                activeSlots[2].GetComponent<Image>().color = Color.grey;
                break;
            case 2:
                activeSlots[0].GetComponent<Image>().color = Color.grey;
                activeSlots[1].GetComponent<Image>().color = Color.grey;
                activeSlots[2].GetComponent<Image>().color = Color.white;
                break;
        }
    }
}
