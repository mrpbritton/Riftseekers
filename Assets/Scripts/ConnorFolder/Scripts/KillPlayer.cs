using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour {

    private void Start() {
        Destroy(gameObject);
        enabled = false;
        //StartCoroutine(murk());
    }

    IEnumerator murk() {
        var temp = FindObjectOfType<PlayerManager>().getTallestParent().gameObject;
        while(temp != null) {
            Destroy(temp.gameObject);
            yield return new WaitForEndOfFrame();
        }
    }
}
