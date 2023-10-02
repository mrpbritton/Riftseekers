using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    [SerializeField] List<Image> activeSlots;
    [SerializeField] List<Image> inactiveSlots;
    [SerializeField] Sprite emptySlotSprite;
    [SerializeField] ConItem tester;

    ItemLibrary il;

    int curIndex = -1;
    int actIndex = 0;

    private void Awake() {
        il = FindObjectOfType<ItemLibrary>();
        Inventory.loadInventory();
        //Inventory.addItem(tester);
        //Inventory.saveInventory();
        show();
        setActIndex(0);
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
        ConItem temp = Inventory.getActiveItem(actIndex, il);
        if(Inventory.getItems(il).Count > curIndex)
            Inventory.overrideActiveItem(actIndex, Inventory.getItems(il)[curIndex]);
        else
            Inventory.removeActiveItem(actIndex);
        if(temp != null)
            Inventory.addItem(temp);
        else
            Inventory.removeItem(curIndex);
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
