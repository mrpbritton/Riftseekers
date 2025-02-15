using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoDamage : MonoBehaviour
{
    [SerializeField, Tooltip("How much damage this object gets")]
    public float damage;
    [SerializeField] public UnityEvent extraRunOnHit = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyHealth enemy))
        {
            enemy.damageTaken(damage, transform.position);
            enemy.GetComponent<EnemyMovement>().slide(transform.position - enemy.transform.position, 10f, .25f);
            if(extraRunOnHit != null && extraRunOnHit.GetPersistentEventCount() > 0)
                extraRunOnHit.Invoke();
        }
    }
}
