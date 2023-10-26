using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPos : MonoBehaviour {
    [SerializeField] GameObject defaultPlayer;

    private void Awake() {
        //  checks if there's not a player in the scene
        if(FindObjectOfType<PlayerManager>() == null) {
            Instantiate(defaultPlayer, transform.position, Quaternion.identity, null);
        }
        else
            FindObjectOfType<PlayerManager>().getTallestParent().transform.position = transform.position;
    }
}
