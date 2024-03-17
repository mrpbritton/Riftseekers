using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class qAbility : Attack
{
    protected override float SetDamage => 12f;

    public override AttackType AType => throw new System.NotImplementedException();
    public override AttackScript AScript => throw new System.NotImplementedException();


    /*public override void attack(InputAction.CallbackContext c)
    {
        Debug.Log("Basic Attack Performed");
    }*/

    public override void DoAttack()
    {
        Debug.Log("Q ability activated");
    }

    public override void ResetAttack()
    {
    }

    public override void Anim(Animator anim, bool reset)
    {
    }
    protected override float SetCooldownTime => 1f;

}