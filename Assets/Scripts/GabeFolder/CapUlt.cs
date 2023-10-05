using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterFrame))]
public class CapUlt : Attack
{
    [SerializeField] float damageMultiplier;
    [SerializeField] float duration;
    [SerializeField] bool bIsActive;
    CharacterFrame frame;

    public override void attack()
    {
        if(frame == null)
            frame = GetComponent<CharacterFrame>();

        if(frame.charge >= frame.chargeLimit && !bIsActive)
        {
            StartCoroutine(Ulting());
        }
    }

    private void Awake()
    {
        
    }

    private void OnDisable()
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
