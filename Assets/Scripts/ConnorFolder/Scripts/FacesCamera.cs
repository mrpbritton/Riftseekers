using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacesCamera : MonoBehaviour {
    Transform camTrans = null;

    private void Awake() {
        if(Camera.main == null || Camera.main.transform == null)
            enabled = false;
        camTrans = Camera.main.transform;
    }

    //  idk if there's a better way to do this
    //  rotates to face the camera
    private void LateUpdate() {
        transform.LookAt(transform.position + camTrans.rotation * Vector3.forward, camTrans.rotation * Vector3.up);
    }
}
