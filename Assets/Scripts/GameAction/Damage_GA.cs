using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_GA : GameAction
{
    [SerializeField]
    private int damage = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Health healthSystem))
        {
            healthSystem.takeDamage(damage);
        }
    }

    public override void Action()
    {
        if (gameObject.TryGetComponent(out Health healthSystem))
        {
            healthSystem.takeDamage(damage);
        }
        else
        {
            Debug.LogWarning("No Health on this object!");
        }
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
