using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    [SerializeField] Image dragged;
    [SerializeField] List<Image> activeSlots;
    [SerializeField] List<Image> inactiveSlots;
    [SerializeField] GameObject background;

    PInput controls;
    PlayerMovement pm;

    int curIndex = -1;
    int actIndex = 0;

    bool draggedState = true;
    bool shown = false;

    public static Action<List<ConItem>> applyActiveItemEffect = delegate { };

    private void Start() {
        pm = FindObjectOfType<PlayerMovement>();
        Inventory.loadInventory();
        hide();
        dragged.gameObject.SetActive(false);
        controls = new PInput();
        controls.Enable();
        controls.UI.Pause.performed += ctx => toggleShown();

        InputManager.switchInput += swapState;
        StartCoroutine(LoadThis());
    }

    IEnumerator LoadThis()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        onLoad();
    }

    private void onLoad()
    {
        AttackManager.I.UpdateAttack();
        /*for(int i = 0; i < 3; i++)
        {
            if(Inventory.getActiveItem(i, il) != null)
            {
                //character.RemoveAbility(Inventory.getActiveItem(i, il));
                character.UpdateAttack(Inventory.getActiveItem(i, il));
            }
        }*/
        var d = SaveData.getString("Bonuses");
        StateSaveData temp = JsonUtility.FromJson<StateSaveData>(d);
        if (string.IsNullOrEmpty(d))
            return;
        for(int i = 0; i < temp.bonuses.Count; i++)
        {
            switch (temp.bonuses[i])
            {
                case CharStats.maxHealth:
                    PlayerStats.UpdateMaxHealth += Mathf.FloorToInt(temp.mods[i]);
                    break;

                case CharStats.health:
                    PlayerStats.UpdateHealth += temp.mods[i];
                    break;

                case CharStats.moveSpeed:
                    PlayerStats.UpdateMovementSpeed += temp.mods[i];
                    break;

                case CharStats.dashSpeed:
                    PlayerStats.UpdateDashSpeed += temp.mods[i];
                    break;

                case CharStats.dashDistance:
                    PlayerStats.UpdateDashDistance += temp.mods[i];
                    break;

                case CharStats.dashCharges:
                    PlayerStats.UpdateDashCharges += Mathf.FloorToInt(temp.mods[i]);
                    break;

                case CharStats.attackDamage:
                    PlayerStats.UpdateAttackDamage += temp.mods[i];
                    break;

                case CharStats.attackSpeed:
                    PlayerStats.UpdateAttackSpeed += temp.mods[i];
                    break;

                case CharStats.cooldownMod:
                    PlayerStats.UpdateCooldownMod += temp.mods[i];
                    break;

                case CharStats.chargeLimit:
                    PlayerStats.UpdateChargeLimit += Mathf.FloorToInt(temp.mods[i]);
                    break;

                default:
                    Debug.LogError("Stat could not be changed.");
                    break;
            }
        }
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
                activeSlots[i].color = i == actIndex ? Color.grey : Color.white;
            for(int i = 0; i < inactiveSlots.Count; i++)
                inactiveSlots[i].color = i == curIndex ? Color.grey : Color.white;
        }

        //Debug.Log(InputManager.isUsingKeyboard());
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
        AttackManager.I.canAttack = false;
        activeSlots[0].GetComponent<Button>().Select();
        //  active items
        for(int i = 0; i < activeSlots.Count; i++) {
            if(Inventory.getActiveItem(i, AugmentLibrary.I) != null) {
                activeSlots[i].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                activeSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = Inventory.getActiveItem(i, AugmentLibrary.I).image;
            }
            else
                activeSlots[i].transform.GetChild(0).GetComponent<Image>().color = Color.clear;
        }

        //  inventory items
        int invCount = Inventory.getItems(AugmentLibrary.I).Count;
        for(int i = 0; i < inactiveSlots.Count; i++) {
            if(i < invCount) {
                inactiveSlots[i].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                inactiveSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = Inventory.getItems(AugmentLibrary.I)[i].image;
            }
            else
                inactiveSlots[i].transform.GetChild(0).GetComponent<Image>().color = Color.clear;
        }

        if(!InputManager.isUsingKeyboard())
            inactiveSlots[0].GetComponent<Button>().Select();
    }
    public void hide() {
        shown = false;
        curIndex = -1;
        actIndex = -1;
        pm.enabled = true;
        AttackManager.I.canAttack = true;
        background.gameObject.SetActive(false);
    }

    void checkActiveItems(int masterInd, Attack.attackType overtype) {
        if(overtype == Attack.attackType.None)
            return;
        for(int i = 2; i >= 0; i--) {
            if(i != masterInd) {
                var it = Inventory.getActiveItem(i, AugmentLibrary.I);
                if(it != null && it.overrideAbil == overtype) { // move back to inv
                    var temp = Inventory.getActiveItem(i, AugmentLibrary.I);
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
        if(ind >= Inventory.getItems(AugmentLibrary.I).Count && InputManager.isUsingKeyboard()) {
            return;
        }

        curIndex = ind;
        if(actIndex > -1)
            swapItems(actIndex);
        if(InputManager.isUsingKeyboard()) {
            draggedState = true;
            dragged.gameObject.SetActive(true);

            dragged.gameObject.SetActive(true);
            dragged.sprite = Inventory.getItems(AugmentLibrary.I)[ind].image;
        }
    }

    public void setActIndex(int ind) {
        actIndex = ind;
        switch(ind) {
            case 0:
                activeSlots[0].color = Color.white;
                activeSlots[1].color = Color.grey;
                activeSlots[2].color = Color.grey;
                break;
            case 1:
                activeSlots[0].color = Color.grey;
                activeSlots[1].color = Color.white;
                activeSlots[2].color = Color.grey;
                break;
            case 2:
                activeSlots[0].color = Color.grey;
                activeSlots[1].color = Color.grey;
                activeSlots[2].color = Color.white;
                break;
        }
    }

    public void swapItems(int actInd) {
        bool differentInds = actInd != actIndex;
        actIndex = actInd;
        if(!InputManager.isUsingKeyboard() && (curIndex == -1 || actIndex == -1))
            return;
        ConItem temp = Inventory.getActiveItem(actIndex, AugmentLibrary.I);   //  saves the active item that's being replaced

        //  checks if the items can be swapped (cannot have 2 or more active items with the same ability override type)
        if(temp != null && differentInds && curIndex != -1)
            checkActiveItems(actInd, temp.overrideAbil);

        //  overrides the active item if there is a valid overrider
        if(Inventory.getItems(AugmentLibrary.I).Count > curIndex && curIndex != -1) {
            Inventory.overrideActiveItem(actIndex, Inventory.getItems(AugmentLibrary.I)[curIndex]);
            Inventory.removeItem(curIndex); //  removes the inventory item because it's not needed anymore
        }
        //  otherwise, just remove the active item
        else
        {
            //AttackManager.IRemoveAbility(Inventory.getActiveItem(actIndex, il));
            Inventory.removeActiveItem(actIndex);
        }

        //  adds the saved active item into the inventory if it's valid
        if (temp != null)
            Inventory.addItem(temp);

        Inventory.saveInventory();
        applyActiveItemEffect(new List<ConItem>() { Inventory.getActiveItem(0, AugmentLibrary.I), Inventory.getActiveItem(1, AugmentLibrary.I), Inventory.getActiveItem(2, AugmentLibrary.I) });
        show();
        actIndex = -1;
        curIndex = -1;
        AttackManager.I.UpdateAttack(Inventory.getActiveItem(actInd, AugmentLibrary.I));
    }
}
