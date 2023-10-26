using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPos : MonoBehaviour {
    [SerializeField] GameObject defaultPlayer;

    bool state = false;

    private void Awake() {
        //  checks if there's not a player in the scene
        if(FindObjectOfType<PlayerManager>() == null) {
            var obj = Instantiate(defaultPlayer, transform.position, Quaternion.identity, null);
            state = false;
        }
        else {
            FindObjectOfType<PlayerManager>().getTallestParent().transform.position = transform.position;
            var obj2 = FindObjectOfType<PlayerManager>();
            obj2.transform.localPosition = Vector3.zero + new Vector3(0f, obj2.transform.lossyScale.y * 2f);
            state = true;
        }
    }

    private void Start() {
        FindObjectOfType<SpawnCam>().thing();
    }
}
