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

    protected override void Start()
    {
        base.Start();
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
        hitbox.GetComponent<DoDamage>().damage = damage;
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
