using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coneSlice : Attack
{
    public GameObject hurtbox;

    new public void Start()
    {
        //hurtbox.active = false;
    }
    public override AttackType AType => throw new System.NotImplementedException();
    public override AttackScript AScript => throw new System.NotImplementedException();


    public override void DoAttack()
    {
        hurtbox.SetActive(true);
        Debug.LogError("Before");
        InBetween();
        Debug.LogError("After");
        hurtbox.SetActive(false);
    }

    public override void ResetAttack()
    {
    }
    public override void Anim(Animator anim, bool reset)
    {
    }
    private IEnumerator InBetween()
    {
        yield return new WaitForSeconds(100000f);
    }

    protected override float SetDamage => 5f;

    protected override float SetCooldownTime => 2f;
}
