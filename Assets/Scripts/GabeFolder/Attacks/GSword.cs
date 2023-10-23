using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSword : Attack
{
    [SerializeField, Tooltip("Used in a calculation to see how much damage dealt")]
    float damage = 1f;
    [SerializeField, Tooltip("Used in a calculation to see how long the cooldown is in seconds")]
    float baseCooldown = .5f;
    [SerializeField, Tooltip("Hitbox the Sword uses")]
    Transform hitbox;
    DoDamage damScript;

    protected override void Start()
    {
        base.Start();
        damScript = hitbox.GetComponent<DoDamage>();
        damScript.damage = damage;
    }
    public override attackType getAttackType()
    {
        return attackType.Secondary;
    }

    protected override float getDamage()
    {
        return damage * frame.attackDamage;
    }

    public override void attack()
    {
        if (damScript.damage != damage)
        {
            damScript.damage = damage;
        }
        cooldownBar.updateSlider(getCooldownTime());
        hitbox.gameObject.SetActive(true);
    }

    public override void reset()
    {
        hitbox.gameObject.SetActive(false);
    }


    protected override float getCooldownTime()
    {
        return baseCooldown / frame.attackSpeed;
    }
}
