using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopPrompter : MonoBehaviour {
    [SerializeField] GameObject canvas;
    [SerializeField] Transform shopHolder, playerHolder;
    List<Image> shopSlots = new List<Image>();
    List<Image> playerSlots = new List<Image>();
    PInput pInput;
    ItemLibrary it;

    bool shown = false;

    InteractUI helperUI;
    Interact interact;

    [SerializeField] string shopName;
    string shopTag() { return "ShopTag:" + shopName; }
    ShopData reference;

    private void Start() {
        pInput = new PInput();
        pInput.Enable();
        pInput.Player.Interact.performed += ctx => toggleShownState();

        interact = GetComponent<Interact>();
        it = FindObjectOfType<ItemLibrary>();

        helperUI = FindObjectOfType<InteractUI>();
        helperUI.addInteractable(transform);

        foreach(var i in shopHolder.GetComponentsInChildren<Button>()) {
            shopSlots.Add(i.transform.GetChild(0).GetComponent<Image>());
        }
        foreach(var i in playerHolder.GetComponentsInChildren<Button>()) {
            playerSlots.Add(i.transform.GetChild(0).GetComponent<Image>());
        }

        //  populates items
        var data = SaveData.getString(shopTag());
        reference = string.IsNullOrEmpty(data) ? new ShopData(FindObjectOfType<ItemLibrary>()) : JsonUtility.FromJson<ShopData>(data);
        SaveData.setString(shopTag(), JsonUtility.ToJson(reference));
        Inventory.loadInventory();
        reshow();

        hide();
    }

    private void OnDisable() {
        pInput.Disable();
    }

    void reshow() {
        //  shop
        for(int i = 0; i < shopSlots.Count; i++) {
            shopSlots[i].sprite = i < reference.items.Count ? reference.items[i].image : null;
        }

        //  player
        for(int i = 0; i < playerSlots.Count; i++) {
            playerSlots[i].sprite = i < Inventory.getItems(it).Count ? Inventory.getItem(i, it).image : null;
        }
    }

    void toggleShownState() {
        if(!interact.inRange())
            return;
        if(shown)
            hide();
        else
            show();
    }

    void show() {
        shown = true;
        helperUI.completeInteraction(transform);
        canvas.SetActive(true);
        FindObjectOfType<PlayerMovement>().enabled = false;
    }
    void hide() {
        shown = false;
        canvas.SetActive(false);
        FindObjectOfType<PlayerMovement>().enabled = true;
        saveShop();
    }

    void saveShop() {
        Inventory.saveInventory();
        SaveData.setString(shopTag(), JsonUtility.ToJson(reference));
    }

    public void buy(int index) {
        if(index >= reference.items.Count)
            return;
        var bought = reference.items[index];
        Inventory.addItem(bought);
        reference.items.RemoveAt(index);
        reshow();
    }
    public void sell(int index) {
        if(index >= Inventory.getItems(it).Count)
            return;
        var sold = Inventory.getItem(index, it);
        reference.items.Add(sold);
        Inventory.removeItem(index);
        reshow();
    }
}

[System.Serializable]
public class ShopData {
    public List<ConItem> items = new List<ConItem>();

    public ShopData(ItemLibrary it) {
        int count = Random.Range(3, 15);
        int itCount = it.getItems().Count;
        for(int i = 0; i < count; i++)
            items.Add(it.getItem(Random.Range(0, itCount)));
    }
}