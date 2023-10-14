using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Bullet_Trail : MonoBehaviour
{
    [SerializeField, Tooltip("Line renderer of the bullet")]
    private LineRenderer line;
    [SerializeField, Tooltip("How many points are in the line renderer")]
    private int points;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        line.positionCount = points;
        line.SetPosition(0, transform.position);
        for(int i = line.positionCount-1; i > 0; i--)
        {
            line.SetPosition(i, line.GetPosition(i-1));  
        }
    }
}
