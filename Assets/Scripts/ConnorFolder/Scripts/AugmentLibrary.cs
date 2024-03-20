using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentLibrary : Singleton<AugmentLibrary>
{
    [SerializeField] List<ConItem> items = new List<ConItem>();
    [SerializeField] List<Augment_SO> augments = new List<Augment_SO>();
    public GameObject itemDrop;

    /// <summary>
    /// Finds an item in the list. Yes, I know this is inefficient. O(n) time complexity.
    /// </summary>
    /// <returns>Index of item if found, -1 if not found</returns>
    private int FindItem(AttackScript aScript)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].attackScript == aScript)
            {
                return i;
            }
        }
        return -1;
    }

    public void SetItemDrop(AttackScript aScript)
    {
        var itemScript = itemDrop.GetComponent<SpitItem_GA>();
        var sprite = itemDrop.GetComponentInChildren<SpriteRenderer>();
        int index = FindItem(aScript);

        if(index < 0)
        {
            Debug.LogError($"Item {aScript} could not be found. Are you sure it's in there?");
            return;
        }

        itemScript.item = items[index];
        itemScript.ability = aScript;
        sprite.sprite = items[index].image;
    }

    public Augment_SO getAugment(int ind) {
        return augments[ind];
    }
    public Augment_SO getAugment(Augment_SO.augmentType type) {
        foreach(var i in augments) {
            if(i.type == type) return i;
        }
        return null;
    }
    public ConItem getItem(int ind) {
        return items[ind];
    }
    public List<Augment_SO> getAugments() { return augments; }
    public List<ConItem> getItems() {  return items; }
}
