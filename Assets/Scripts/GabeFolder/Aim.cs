using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    [SerializeField]
    private float detectionDistance;
    private Ray range;
    [SerializeField]
    private Transform originPosition;
    [SerializeField]
    private Camera cam;

    
    private Vector3 cursorPos;
    private Vector3 cursorWorldPos;

    private void Update()
    {
        //cursor into world space
        cursorPos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(cursorPos);
        cursorWorldPos = ray.GetPoint(10);

        //raycast
        range.direction = new Vector3(cursorWorldPos.x - originPosition.position.x, 
                                      cursorWorldPos.y - originPosition.position.y + transform.localScale.y, 
                                      cursorWorldPos.z - originPosition.position.z);
        range.origin = transform.position;
        Debug.DrawRay(range.origin, range.direction.normalized * detectionDistance, Color.red);

        //rotation
        float angle = Mathf.Atan2(range.direction.y, range.direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0));
    }
}
