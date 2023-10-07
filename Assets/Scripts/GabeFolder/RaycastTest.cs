using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTest : MonoBehaviour
{
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 20, Color.red);
    }

}
