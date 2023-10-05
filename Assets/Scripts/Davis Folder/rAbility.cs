using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coneSlice : Attack
{
    public GameObject hurtbox;

    public void Start()
    {
        //hurtbox.active = false;
    }
    public override attackType getAttackType()
    {
        return attackType.FAbility;
    }

    public override void attack()
    {
        hurtbox.SetActive(true);
        Debug.LogError("Before");
        inBetween();
        Debug.LogError("After");
        hurtbox.SetActive(false);
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
