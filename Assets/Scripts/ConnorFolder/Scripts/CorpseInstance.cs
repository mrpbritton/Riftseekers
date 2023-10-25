using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseInstance : MonoBehaviour {

    private void OnCollisionEnter(Collision col) {
        if(col.gameObject.tag == "Player")
            Debug.Log("worked");
    }
}
