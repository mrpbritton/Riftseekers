using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPos : MonoBehaviour {
    [SerializeField] GameObject defaultPlayer;
    [SerializeField] float spawnHeight =1f;

    bool state = false;

    private void Awake() {
        //  checks if there's not a player in the scene
        if(FindObjectOfType<PlayerManager>() == null) {
            var obj = Instantiate(defaultPlayer, transform.position + Vector3.up * spawnHeight, Quaternion.identity, null);
            state = false;
        }
        else {
            FindObjectOfType<PlayerManager>().getTallestParent().transform.position = transform.position + Vector3.up * spawnHeight;
            var obj2 = FindObjectOfType<PlayerManager>();
            obj2.transform.localPosition = Vector3.zero;
            state = true;
        }
    }

    private void Start() {
        FindObjectOfType<SpawnCam>().thing();
    }
}
