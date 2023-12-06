using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterFrame))]
public class CapUlt : Attack
{
    [SerializeField] float damageMultiplier;
    [SerializeField] float duration;
    [SerializeField] bool bIsActive;

    public override void attack()
    {
        if(frame.charge >= frame.chargeLimit && !bIsActive)
        {
            StartCoroutine(Ulting());
        }
    }

    public override void reset()
    {
        //
    }

    private void Awake()
    {
        
    }

    public override void anim(Animator anim, bool reset)
    {
    }
    private void ManageCharge()
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

    IEnumerator Ulting()
    {
        bIsActive = true;
        yield return new WaitForSeconds(duration);
        bIsActive = false;
    }
}
