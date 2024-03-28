using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Damage_GA : GameAction
{
    [SerializeField]
    public float originalDamage = 5f;
    [SerializeField]
    public float damage = 5f;

    private void OnTriggerEnter(Collider other)
    {
//        other.gameObject.GetComponentInParent<Health>().takeDamage(damage);

        if (other.TryGetComponent(out Health healthSystem))
        {
            healthSystem.takeDamage(damage);
        }
    }

    public override void Action()
    {
//        transform.parent.gameObject.GetComponent<Health>().takeDamage(damage);
        /*
        if (gameObject.TryGetComponent(out Health healthSystem))
        {
            healthSystem.takeDamage(damage);
        }
        else
        {
            Debug.LogWarning("No Health on this object!");
        }
        */
    }
    public override void DeAction()
    {
        //nothing
    }
    public override void ResetToDefault()
    {
        //nothing
    }
}
