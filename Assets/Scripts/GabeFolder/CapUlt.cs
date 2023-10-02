using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapUlt : Attack
{
    [SerializeField] float damageMultiplier;
    [SerializeField] float duration;
    [SerializeField] bool bIsActive;

    public override void attack()
    {

    }
    public override attackType getAttackType()
    {
        return attackType.FAbility;
    }
    protected override float getDamage()
    {
        return 30f;
    }
    protected override float getCooldownTime()
    {
        return 3f;
    }
}
