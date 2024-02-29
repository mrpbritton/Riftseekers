using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentLibrary : Singleton<AugmentLibrary>
{
    [SerializeField] List<ConItem> items = new List<ConItem>();
    [SerializeField] List<Augment_SO> augments = new List<Augment_SO>();
    
    [SerializeField] GameObject itemDrop;

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
