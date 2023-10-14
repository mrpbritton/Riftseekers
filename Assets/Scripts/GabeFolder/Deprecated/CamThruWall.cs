using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamThruWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("wall"))
        {
            var mesh = other.gameObject.GetComponent<MeshRenderer>();
            mesh.enabled = !mesh.enabled;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("wall"))
        {
            var mesh = other.gameObject.GetComponent<MeshRenderer>();
            mesh.enabled = !mesh.enabled; //flip it
        }
    }
}
