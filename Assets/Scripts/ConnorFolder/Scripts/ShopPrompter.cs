using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopPrompter : MonoBehaviour {
    [SerializeField] GameObject canvas;
    [SerializeField] Transform shopHolder, playerHolder;
    List<Image> shopSlots = new List<Image>();
    List<Image> playerSlots = new List<Image>();
    PInput pInput;
    [SerializeField] TextMeshProUGUI moneyText;

    bool shown = false;

    InteractUI helperUI;
    Interact interact;

    [SerializeField] string shopName;
    string shopTag() { return "ShopTag:" + shopName; }
    ShopData reference;

    [SerializeField] Sprite loreSprite;

    private void Start() {
        pInput = new PInput();
        pInput.Enable();
        pInput.Player.Interact.performed += ctx => toggleShownState();

        interact = GetComponent<Interact>();

        helperUI = FindObjectOfType<InteractUI>();
        helperUI.addInteractable(transform, new InteractInfo(shopName, InteractInfo.interactType.Shop));

        foreach(var i in shopHolder.GetComponentsInChildren<Button>()) {
            shopSlots.Add(i.transform.GetChild(0).GetComponent<Image>());
        }
        foreach(var i in playerHolder.GetComponentsInChildren<Button>()) {
            playerSlots.Add(i.transform.GetChild(0).GetComponent<Image>());
        }

        //  populates items
        var data = SaveData.getString(shopTag());
        reference = string.IsNullOrEmpty(data) ? new ShopData(Random.Range(2,5), Random.Range(3, 6)) : JsonUtility.FromJson<ShopData>(data);
        SaveData.setString(shopTag(), JsonUtility.ToJson(reference));
        Inventory.loadInventory();
        reshow();

        hide();
    }

    private void OnDisable() {
        pInput.Disable();
    }

    void reshow() {
        moneyText.text = "Money: <color=yellow>" + Inventory.getMoney().ToString();
        //  shop
        for(int i = 0; i < shopSlots.Count; i++) {
            if(i < reference.items.Count) {
                shopSlots[i].sprite = reference.items[i].image;
                shopSlots[i].transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = reference.items[i].value.ToString();
            }
            else if(i - reference.items.Count < reference.loreInds.Count) {
                var l = AugmentLibrary.I.getLore(reference.loreInds[i - reference.items.Count]);
                shopSlots[i].sprite = loreSprite;
                shopSlots[i].transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = l.title;
            }
            else {
                shopSlots[i].sprite = null;
                shopSlots[i].transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }

        //  player
        for(int i = 0; i < playerSlots.Count; i++) {
            if(i < Inventory.getItems(AugmentLibrary.I).Count) {
                playerSlots[i].sprite = i < Inventory.getItems(AugmentLibrary.I).Count ? Inventory.getItem(i, AugmentLibrary.I).image : null;
                playerSlots[i].transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = Inventory.getItem(i, AugmentLibrary.I).value.ToString();
            }
            else {
                playerSlots[i].sprite = null;
                playerSlots[i].transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    void toggleShownState() {
        if(!interact.inRange())
            return;
        if(shown)
            hide();
        else
            show();
        Time.timeScale = shown ? 0f : 1f;
    }

    void show() {
        shown = true;
        reshow();
        helperUI.completeInteraction(transform);
        canvas.SetActive(true);
        FindObjectOfType<PlayerMovement>().enabled = false;
        AttackManager.I.enabled = false;
    }
    void hide() {
        shown = false;
        canvas.SetActive(false);
        FindObjectOfType<PlayerMovement>().enabled = true;
        AttackManager.I.enabled = true;
        saveShop();
    }

    void saveShop() {
        Inventory.saveInventory();
        SaveData.setString(shopTag(), JsonUtility.ToJson(reference));
    }

    public void buy(int index) {
        if(index < reference.items.Count) {
            var bought = reference.items[index];
            //  checks if player has enough money
            if(Inventory.getMoney() < bought.value) {
                shopSlots[index].transform.parent.GetComponent<Image>().color = Color.red;
                shopSlots[index].transform.parent.GetComponent<Image>().DOColor(Color.white, .25f);
                return;
            }
            Inventory.addItem(bought);
            Inventory.changeMoney(-bought.value);
            reference.items.RemoveAt(index);
        }
        else if(index < reference.items.Count + reference.loreInds.Count) {
            var bought = AugmentLibrary.I.getLore(reference.loreInds[index - reference.items.Count]);

            if(Inventory.getMoney() < bought.value) {
                shopSlots[index].transform.parent.GetComponent<Image>().color = Color.red;
                shopSlots[index].transform.parent.GetComponent<Image>().DOColor(Color.white, .25f);
                return;
            }
            Inventory.removeLoreIndex(reference.loreInds[index - reference.items.Count]);
            Inventory.changeMoney(-bought.value);
            reference.loreInds.RemoveAt(index - reference.items.Count);
        }
        else return;
        reshow();
    }
    public void sell(int index) {
        if(index >= Inventory.getItems(AugmentLibrary.I).Count)
            return;
        var sold = Inventory.getItem(index, AugmentLibrary.I);
        reference.items.Add(sold);
        Inventory.removeItem(index);
        Inventory.changeMoney(sold.value);
        reshow();
    }
}

[System.Serializable]
public class ShopData {
    public List<ConItem> items = new List<ConItem>();
    public List<int> loreInds = new List<int>();

    public ShopData(int itCount, int loCount) {
        for(int i = 0; i < itCount; i++)
            items.Add(AugmentLibrary.I.getRandItem());

        for(int i = 0; i < loCount; i++)
            loreInds.Add(Inventory.getRandLoreIndex());
    }
}