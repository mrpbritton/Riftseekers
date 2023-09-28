using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthCanvas : MonoBehaviour {

    //  idk if there's a better way to do this
    //  rotates to face the camera
    private void LateUpdate() {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
}
