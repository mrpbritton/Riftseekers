using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicAttack : Attack
{
    protected override float getDamage()
    {
        return 12f;
    }
    public override attackType getAttackType()
    {
        return attackType.Basic;
    }
    /*public override void attack(InputAction.CallbackContext c)
    {
        Debug.Log("Basic Attack Performed");
    }*/

    public override void attack()
    {
        throw new System.NotImplementedException();
    }
    protected override float getCooldownTime() {
        return 1f;
    }

}
