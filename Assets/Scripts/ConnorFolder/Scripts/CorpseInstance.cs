using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseInstance : MonoBehaviour {

    [SerializeField] Collider mainCol;

    private void OnCollisionEnter(Collision col) {
        if(col.gameObject.tag == "Player")
            Debug.Log("worked");
    }
}
