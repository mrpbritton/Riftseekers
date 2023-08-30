using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTest : MonoBehaviour
{
    public Vector3 ScreenPosition;
    public Vector3 WorldPosition;

    void Update()
    {
        ScreenPosition = Input.mousePosition;
        ScreenPosition.z = Camera.main.nearClipPlane + 21.5f;

        WorldPosition = Camera.main.ScreenToWorldPoint(ScreenPosition);

        transform.position = WorldPosition;
    }
}
