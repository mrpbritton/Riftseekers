using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : GameAction
{
    [SerializeField]
    private int damage = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyHealth healthSystem))
        {
            healthSystem.damageTaken(damage);
        }
    }
}
