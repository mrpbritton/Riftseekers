using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracking : MonoBehaviour
{
    
    public LayerMask mask;
    RaycastHit hitInfo;
    


    void Update()
    {
   
        //This script MUST be on the empty mouse tracker GO, and the mouse tracker should have its collider disabled. 

        //finds ray from camera to place arrow is pointing
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       

        
        if (Physics.Raycast(ray, out hitInfo, 100f, mask.value))
        {
           
            if (hitInfo.point == null)
                Debug.Log("Out of Bounds");
        }
        transform.position = hitInfo.point;
    }
}
