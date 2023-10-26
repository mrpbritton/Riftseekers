using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    bool initted = false;

    private void Awake() {
        var tallestParent = gameObject;
        while(tallestParent.transform.parent != null)
            tallestParent = tallestParent.transform.parent.gameObject;
        //  checks if solo
        if(FindObjectsOfType<PlayerManager>().Length > 1 && !initted) {
            Debug.Log("connor happy");
            Destroy(tallestParent);
        }

        //  if solo init
        initted = true;
        DontDestroyOnLoad(tallestParent);
    }
}
