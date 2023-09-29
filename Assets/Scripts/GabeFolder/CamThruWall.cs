using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamThruWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if(other.CompareTag("wall"))
        {
            var mesh = other.gameObject.GetComponent<MeshRenderer>();
            mesh.enabled = !mesh.enabled; //flip it
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("BBBBBBBB");
        if (other.CompareTag("wall"))
        {
            var mesh = other.gameObject.GetComponent<MeshRenderer>();
            mesh.enabled = !mesh.enabled; //flip it
        }
    }
}
