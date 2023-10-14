using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField, Tooltip("Speed of the projectile")]
    private float speed;
    [Tooltip("Direction the projectile is headed")]
    public Vector3 direction;
    [Tooltip("Damage of the projectile")]
    public float damage;

    private void Start()
    {
        
    }
}
