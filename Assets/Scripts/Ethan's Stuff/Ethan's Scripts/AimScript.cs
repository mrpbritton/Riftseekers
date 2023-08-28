using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AimScript : MonoBehaviour
{
    private Vector3 mousePos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LookAtMouse();
    }
    public void LookAtMouse()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log("MousePos:" + mousePos);
        transform.LookAt(new Vector3(mousePos.x, transform.position.y, mousePos.z));
    }
}
