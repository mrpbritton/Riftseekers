using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class qAbility : Attack
{
    protected override float getDamage()
    {
        return 12f;
    }

    public override AttackType AType => throw new System.NotImplementedException();
    public override AttackScript AScript => throw new System.NotImplementedException();


    /*public override void attack(InputAction.CallbackContext c)
    {
        Debug.Log("Basic Attack Performed");
    }*/

    public override void attack()
    {
        Debug.Log("Q ability activated");
    }

    public override void reset()
    {
    }

    public override void anim(Animator anim, bool reset)
    {
    }
    protected override float getCooldownTime() {
        return 1f;
    }

}