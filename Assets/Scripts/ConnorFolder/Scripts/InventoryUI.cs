using System;
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

    public static Action<List<ConItem>> applyActiveItemEffect = delegate { };

    private void Start() {
        il = FindObjectOfType<ItemLibrary>();
        pm = FindObjectOfType<PlayerMovement>();
        Inventory.loadInventory();
        hide();
        dragged.gameObject.SetActive(false);
        controls = new PInput();
        controls.Enable();
        controls.UI.Pause.performed += ctx => toggleShown();

        InputManager.switchInput += swapState;
    }

    private void LateUpdate() {
        if(InputManager.isUsingKeyboard()) {
            if(draggedState)
                dragged.transform.position = Input.mousePosition;
            if(draggedState && Input.GetMouseButtonDown(0)) {
                draggedState = false;
                dragged.gameObject.SetActive(false);
            }
        }
        else {
            for(int i = 0; i < activeSlots.Count; i++)
                activeSlots[i].GetComponent<Image>().color = i == actIndex ? Color.grey : Color.white;
            for(int i = 0; i < inactiveSlots.Count; i++)
                inactiveSlots[i].GetComponent<Image>().color = i == curIndex ? Color.grey : Color.white;
        }
    }

    private void OnDisable() {
        if(controls != null)
            controls.Disable();
        InputManager.switchInput -= swapState;
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

        if(!InputManager.isUsingKeyboard())
            inactiveSlots[0].GetComponent<Button>().Select();
    }
    void hide() {
        shown = false;
        curIndex = -1;
        actIndex = -1;
        pm.enabled = true;
        background.gameObject.SetActive(false);
    }

    void checkActiveItems(int masterInd, Attack.attackType overtype) {
        if(overtype == Attack.attackType.None)
            return;
        for(int i = 2; i >= 0; i--) {
            if(i != masterInd) {
                var it = Inventory.getActiveItem(i, il);
                if(it != null && it.overrideAbil == overtype) { // move back to inv
                    var temp = Inventory.getActiveItem(i, il);
                    Inventory.removeActiveItem(i);
                    Inventory.addItem(temp);
                }
            }
        }
    }


    void swapState(bool usingKeyboard) {
        curIndex = -1;
        actIndex = -1;
        dragged.gameObject.SetActive(false);
        dragged.sprite = null;
        if(!usingKeyboard) {
            inactiveSlots[0].GetComponent<Button>().Select();
        }
    }

    public void setCurIndex(int ind) {
        if(ind >= Inventory.getItems(il).Count && InputManager.isUsingKeyboard()) {
            return;
        }

        curIndex = ind;
        if(actIndex > -1)
            swapItems(actIndex);
        if(InputManager.isUsingKeyboard()) {
            draggedState = true;
            dragged.gameObject.SetActive(true);

            dragged.gameObject.SetActive(true);
            dragged.sprite = Inventory.getItems(il)[ind].image;
        }
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
        bool differentInds = actInd != actIndex;
        actIndex = actInd;
        if(!InputManager.isUsingKeyboard() && (curIndex == -1 || actIndex == -1))
            return;
        ConItem temp = Inventory.getActiveItem(actIndex, il);   //  saves the active item that's being replaced

        //  checks if the items can be swapped (cannot have 2 or more active items with the same ability override type)
        if(temp != null && differentInds && curIndex != -1)
            checkActiveItems(actInd, temp.overrideAbil);

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
        applyActiveItemEffect(new List<ConItem>() { Inventory.getActiveItem(0, il), Inventory.getActiveItem(1, il), Inventory.getActiveItem(2, il) });
        show();
        actIndex = -1;
        curIndex = -1;
    }
}
