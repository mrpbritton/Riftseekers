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


    public override void attack()
    {
        hurtbox.SetActive(true);
        Debug.LogError("Before");
        inBetween();
        Debug.LogError("After");
        hurtbox.SetActive(false);
    }

    public override void reset()
    {
    }
    public override void anim(Animator anim, bool reset)
    {
    }
    private IEnumerator inBetween()
    {
        yield return new WaitForSeconds(100000f);
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
