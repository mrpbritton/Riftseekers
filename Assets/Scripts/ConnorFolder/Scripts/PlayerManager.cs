using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    private void Awake() {
        var tallestParent = gameObject;
        while(tallestParent.transform.parent != null)
            tallestParent = tallestParent.transform.parent.gameObject;
        DontDestroyOnLoad(tallestParent);
    }
}
