using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class AimScript : MonoBehaviour
{
    public GameObject Mouse;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(Mouse.transform.position.x,1.5f , Mouse.transform.position.z));
    }
}
