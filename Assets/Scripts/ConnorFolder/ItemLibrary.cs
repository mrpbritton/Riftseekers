using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLibrary : MonoBehaviour {
    [SerializeField] List<ConItem> items = new List<ConItem>();

    public ConItem getItem(int ind) {
        return items[ind];
    }
    public List<ConItem> getItems() {  return items; }
}
