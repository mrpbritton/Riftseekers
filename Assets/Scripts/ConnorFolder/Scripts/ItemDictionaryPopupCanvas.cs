using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionaryPopupCanvas : Singleton<ItemDictionaryPopupCanvas> {
    [SerializeField] GameObject popupPref;
    [SerializeField] float offset;

    Transform playerTrans;

    private void Start() {
        playerTrans = FindObjectOfType<PlayerMovement>().transform;
    }

    public void showForLore() {
        var pp = Instantiate(popupPref, transform);
        pp.transform.position = playerTrans.position + Vector3.up * offset;
        Destroy(pp.gameObject, 1f);
    }
}
