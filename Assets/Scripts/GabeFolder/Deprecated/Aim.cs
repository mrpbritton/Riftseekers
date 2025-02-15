using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    [SerializeField]
    private float detectionDistance;
    [SerializeField]
    private Camera cam;
    
    private Vector3 cursorPos;
    private Vector3 cursorDir;

    private void Update()
    {
        //cursor into world space
        cursorPos = Input.mousePosition;

        //puts the cursor direction vector in the middle of the screen
        cursorDir = new Vector3(cursorPos.x - Screen.width / 2, cursorPos.y - Screen.height / 2, 0);

        //rotation that points somewhere around the middle of the screen
        float angle = Mathf.Atan2(cursorDir.y, cursorDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0));
    }
}
