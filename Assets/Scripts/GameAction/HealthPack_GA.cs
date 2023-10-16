using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack_GA : GameAction
{
    private Health health;
    private float healAmount;
    [SerializeField]
    private float healPercent = 0.25f;
    public override void Action()
    {
        if (health == null)
        {
            health = FindObjectOfType<Health>();
        }
        if (health.health == health.maxHealth) return;
        healAmount = health.maxHealth * healPercent;
        health.heal(healAmount);
        Destroy(this.gameObject);
    }
    public override void DeAction()
    {
    }
    public override void ResetToDefault()
    {
    }
}
