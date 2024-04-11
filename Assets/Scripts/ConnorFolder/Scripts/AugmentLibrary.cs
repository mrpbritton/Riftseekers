using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AugmentLibrary : Singleton<AugmentLibrary> {
    [Header("Items")]
    [SerializeField] List<ConItem> mundaneItems = new List<ConItem>();
    [SerializeField] List<ConItem> abnormalItems = new List<ConItem>();
    [SerializeField] List<ConItem> remarkableItems = new List<ConItem>();
    [SerializeField] List<ConItem> fabulousItems = new List<ConItem>();

    [Header("Augments")]
    [SerializeField] List<Augment_SO> mundaneAugments = new List<Augment_SO>();
    [SerializeField] List<Augment_SO> abnormalAugments = new List<Augment_SO>();
    [SerializeField] List<Augment_SO> remarkableAugments = new List<Augment_SO>();
    [SerializeField] List<Augment_SO> fabulousAugments = new List<Augment_SO>();
    [SerializeField] GameObject itemDrop;

    [SerializeField] Material mundaneMat, abnormalMat, remarkableMat, fabulousMat;

    [Header("Lore Drops")]
    [SerializeField] List<LorePiece_SO> remainingLore = new();
    [SerializeField] GameObject loreDrop;
    public enum rarity {
        None, Mundane, Abnormal, Remarkable, Fabulous
    }

    const int mundaneThreshDrop = 25, abnormalThreshDrop = 20, remarkableThreshDrop = 15, fabulousThreshDrop = 10;
    const int abilityDrop = 19;

    /// <summary>
    /// Finds an item in the list. Yes, I know this is inefficient. O(n) time complexity.
    /// </summary>
    /// <returns>Index of item if found, -1 if not found</returns>
    public int FindIndex(AttackScript aScript) {
        for(int i = 0; i < mundaneItems.Count; i++) {
            if(mundaneItems[i].attackScript == aScript)
                return i;
        }
        for(int i = 0; i < abnormalItems.Count; i++) {
            if(mundaneItems[i].attackScript == aScript)
                return i;
        }
        for(int i = 0; i < remarkableItems.Count; i++) {
            if(mundaneItems[i].attackScript == aScript)
                return i;
        }
        for(int i = 0; i < fabulousItems.Count; i++) {
            if(mundaneItems[i].attackScript == aScript)
                return i;
        }
        return -1;
    }
    
    public GameObject GetItemDrop()
    {
        return itemDrop;
    }

    public ConItem FindItem(AttackScript aScript) {
        foreach(var i in mundaneItems) {
            if(i.attackScript == aScript)
                return i;
        }
        foreach(var i in abnormalItems) {
            if(i.attackScript == aScript)
                return i;
        }
        foreach(var i in remarkableItems) {
            if(i.attackScript == aScript)
                return i;
        }
        foreach(var i in fabulousItems) {
            if(i.attackScript == aScript)
                return i;
        }
        return null;
    }

    public void SetItemDrop(AttackScript aScript) {
        var itemScript = itemDrop.GetComponent<SpitItem_GA>();
        var itemUI = itemDrop.GetComponent<UpdatePUI_GA>();
        var sprite = itemDrop.GetComponentInChildren<SpriteRenderer>();
        var temp = FindItem(aScript);

        if(temp == null) {
            Debug.LogError("Item does not exist");
            return;
        }

        itemScript.item = temp;
        itemScript.ability = aScript;
        itemUI.item = temp;
        sprite.sprite = temp.image;
    }

    public ConItem getRandItem() {
        int rand = Random.Range(0, mundaneThreshDrop + abnormalThreshDrop + remarkableThreshDrop + fabulousThreshDrop);
        if(rand < mundaneThreshDrop) return mundaneItems[Random.Range(0, mundaneItems.Count)];
        if(rand < abnormalThreshDrop) return abnormalItems[Random.Range(0, abnormalItems.Count)];
        if(rand < remarkableThreshDrop) return remarkableItems[Random.Range(0, remarkableItems.Count)];
        else return fabulousItems[Random.Range(0, fabulousItems.Count)];
    }

    public Augment_SO getAugment(Augment_SO.augmentType type) {
        foreach(var i in mundaneAugments) {
            if(i.type == type) return i;
        }
        foreach(var i in abnormalAugments) {
            if(i.type == type) return i;
        }
        foreach(var i in remarkableAugments) {
            if(i.type == type) return i;
        }
        foreach(var i in fabulousAugments) {
            if(i.type == type) return i;
        }
        return null;
    }
    public Augment_SO getRandAugment() {
        int rand = Random.Range(0, mundaneThreshDrop + abnormalThreshDrop + remarkableThreshDrop + fabulousThreshDrop);
        if(rand < mundaneThreshDrop) return mundaneAugments[Random.Range(0, mundaneAugments.Count)];
        if(rand < abnormalThreshDrop) return abnormalAugments[Random.Range(0, abnormalAugments.Count)];
        if(rand < remarkableThreshDrop) return remarkableAugments[Random.Range(0, remarkableAugments.Count)];
        else return fabulousAugments[Random.Range(0, fabulousAugments.Count)];
    }

    public int getAugmentCount() {
        return mundaneAugments.Count + abnormalAugments.Count + remarkableAugments.Count + fabulousAugments.Count;
    }
    public int getItemCount() {
        return mundaneItems.Count + abnormalItems.Count + remarkableItems.Count + fabulousItems.Count;
    }

    public rarity getItemRarity(AttackScript aScript) {
        foreach(var i in mundaneItems) 
            if(i.attackScript == aScript) return rarity.Mundane;
        foreach(var i in abnormalItems)
            if(i.attackScript == aScript) return rarity.Abnormal;
        foreach(var i in remarkableItems) 
            if(i.attackScript == aScript) return rarity.Remarkable;
        foreach(var i in fabulousItems)
            if(i.attackScript == aScript) return rarity.Fabulous;
        return rarity.None;
    }
    public Material getItemRarityMat(AttackScript aScript) {
        var rar = getItemRarity(aScript);
        return rar == rarity.Mundane ? mundaneMat : rar == rarity.Abnormal ? abnormalMat : rar == rarity.Remarkable ? remarkableMat : rar == rarity.Fabulous ? fabulousMat : null;
    }

    public List<ConItem> getAllItems() {
        var temp = new List<ConItem>();
        temp.AddRange(mundaneItems);
        temp.AddRange(abnormalItems);
        temp.AddRange(remarkableItems);
        temp.AddRange(fabulousItems);
        return temp;
    }
    public List<Augment_SO> getAllAugments() {
        var temp = new List<Augment_SO>();
        temp.AddRange(mundaneAugments);
        temp.AddRange(abnormalAugments);
        temp.AddRange(remarkableAugments);
        temp.AddRange(fabulousAugments);
        return temp;
    }

    public GameObject GetLoreItem()
    {
        return loreDrop;
    }

    public LorePiece_SO GetRandLore()
    {
        return remainingLore[Random.Range(0, remainingLore.Count)];
    }

    public void RemoveLore(LorePiece_SO tmp)
    {
        remainingLore.Remove(tmp);
    }
}
