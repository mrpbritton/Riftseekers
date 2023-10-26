using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    [SerializeField] Image dragged;
    [SerializeField] List<Image> activeSlots;
    [SerializeField] List<Image> inactiveSlots;
    [SerializeField] Sprite emptySlotSprite;
    [SerializeField] GameObject background;

    ItemLibrary il;
    PInput controls;
    PlayerMovement pm;

    int curIndex = -1;
    int actIndex = 0;

    bool draggedState = true;
    bool shown = false;

    private void Start() {
        il = FindObjectOfType<ItemLibrary>();
        pm = FindObjectOfType<PlayerMovement>();
        Inventory.loadInventory();
        hide();
        dragged.gameObject.SetActive(false);
        controls = new PInput();
        controls.Enable();
        controls.UI.Pause.performed += ctx => toggleShown();
        //setActIndex(0);
    }

    private void LateUpdate() {
        if(draggedState)
            dragged.transform.position = Input.mousePosition;
        if(draggedState && Input.GetMouseButtonDown(0)) {
            draggedState = false;
            dragged.gameObject.SetActive(false);
        }
    }

    private void OnDisable() {
        if(controls != null)
            controls.Disable();
    }

    void toggleShown() {
        if(shown)
            hide();
        else
            show();
    }

    public void show() {
        shown = true;
        background.gameObject.SetActive(true);
        pm.enabled = false;
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
    void hide() {
        shown = false;
        curIndex = -1;
        actIndex = -1;
        pm.enabled = true;
        background.gameObject.SetActive(false);
    }

    public void setCurIndex(int ind) {
        if(ind >= Inventory.getItems(il).Count) {
            return;
        }

        curIndex = ind;
        draggedState = true;
        dragged.gameObject.SetActive(true);

        dragged.gameObject.SetActive(true);
        dragged.sprite = Inventory.getItems(il)[ind].image;
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

    public void swapItems(int actInd) {
        actIndex = actInd;
        ConItem temp = Inventory.getActiveItem(actIndex, il);   //  saves the active item that's being replaced
        //  overrides the active item if there is a valid overrider
        if(Inventory.getItems(il).Count > curIndex && curIndex != -1) {
            Inventory.overrideActiveItem(actIndex, Inventory.getItems(il)[curIndex]);
            Inventory.removeItem(curIndex); //  removes the inventory item because it's not needed anymore
        }
        //  otherwise, just remove the active item
        else
            Inventory.removeActiveItem(actIndex);

        //  adds the saved active item into the inventory if it's valid
        if(temp != null)
            Inventory.addItem(temp);
        Inventory.saveInventory();
        show();
        curIndex = -1;
    }
}
