using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicAttack : Attack
{
    protected override float SetDamage => 12f;

    public override AttackType AType => throw new System.NotImplementedException();
    public override AttackScript AScript => throw new System.NotImplementedException();


    /*public override void attack(InputAction.CallbackContext c)
    {
        Debug.Log("Basic Attack Performed");
    }*/

    public override void Anim(Animator anim, bool reset)
    {
    }
    public override void DoAttack()
    {
        throw new System.NotImplementedException();
    }

    public override void ResetAttack()
    {
    }
    protected override float SetCooldownTime => 1f;

}
