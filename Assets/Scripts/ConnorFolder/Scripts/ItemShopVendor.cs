using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShopVendor : ShopVendor {

    protected override void interact() {
        Debug.Log("Buy Something");
    }

    protected override void deinteract() {
        Debug.Log("Leave me");
    }
}
