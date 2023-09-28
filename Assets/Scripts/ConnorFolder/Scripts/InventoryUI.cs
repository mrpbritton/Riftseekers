using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    [SerializeField] List<Image> activeSlots;
    [SerializeField] List<Image> inactiveSlots;
    [SerializeField] Sprite emptySlotSprite;

    ItemLibrary il;

    private void Awake() {
        SaveData.wipe();
        il = FindObjectOfType<ItemLibrary>();
        show();
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
}
