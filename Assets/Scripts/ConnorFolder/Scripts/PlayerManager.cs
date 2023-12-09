using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public GameObject getTallestParent() {
        var tallestParent = gameObject;
        while(tallestParent.transform.parent != null)
            tallestParent = tallestParent.transform.parent.gameObject;
        return tallestParent;
    }
}
