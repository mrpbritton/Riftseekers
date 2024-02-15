using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentLibrary : MonoBehaviour {
    [SerializeField] List<ConItem> items = new List<ConItem>();
    [SerializeField] List<ConItem> augments = new List<ConItem>();
    
    [SerializeField] GameObject itemDrop;

    public ConItem getAugment(int ind) {
        return augments[ind];
    }
    public ConItem getItem(int ind) {
        return items[ind];
    }
    public List<ConItem> getAugments() { return augments; }
    public List<ConItem> getItems() {  return items; }
}
