using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackManager))]
public class CapUlt : Attack
{
    [SerializeField] float damageMultiplier;
    [SerializeField] float duration;
    [SerializeField] bool bIsActive;
    public override AttackType AType => throw new System.NotImplementedException();
    public override AttackScript AScript => throw new System.NotImplementedException();

    public override void DoAttack()
    {
    }

    public override void ResetAttack()
    {
        //
    }

    private void Awake()
    {
        if (bIsActive) return; //I just did this to get rid of a warning -Gabe
    }

    public override void Anim(Animator anim, bool reset)
    {
    }
    private void ManageCharge()
    {

    }

    protected override float SetDamage => 30f;
    protected override float SetCooldownTime => 3f;


    IEnumerator Ulting()
    {
        bIsActive = true;
        yield return new WaitForSeconds(duration);
        bIsActive = false;
    }
}
