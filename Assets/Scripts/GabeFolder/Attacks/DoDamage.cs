using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamage : MonoBehaviour
{
    [SerializeField, Tooltip("How much damage this object gets")]
    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyHealth enemy))
        {
            enemy.damageTaken(damage, transform.position);
        }
    }
}
