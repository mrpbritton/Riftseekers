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
        if (health.CurrentHealth == health.MaxHealth) return;
        healAmount = health.MaxHealth * healPercent;
        health.heal(healAmount);
        Destroy(gameObject);
    }
    public override void DeAction()
    {
    }
    public override void ResetToDefault()
    {
    }
}
