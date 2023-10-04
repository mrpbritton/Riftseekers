using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rAbility : Attack
{
    public override attackType getAttackType()
    {
        return attackType.FAbility;
    }

    public override void attack()
    {
        throw new System.NotImplementedException();
    }

    protected override float getDamage()
    {
        return 5f;
    }

    protected override float getCooldownTime()
    {
        return 2f;
    }
}
