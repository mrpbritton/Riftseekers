using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Basic_Proj : Attack {

    [Header("Gun Attributes")]
    [SerializeField, Tooltip("Used in a calculation to see how much damage dealt")]
    float damage = 1f;
    [SerializeField, Tooltip("Used in a calculation to see how long the cooldown is in seconds")]
    float baseCooldown = 3f;
    [Header("Bullet Attributes")]
    [SerializeField, Tooltip("Where the bullet instantiates")]
    Transform origin;
    [SerializeField, Tooltip("Bullet that gets spawned")]
    Transform bullet;
    
    bool bCanShoot = true;

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
        Vector3 dir = Attack.GetPoint();
        Vector3 direction = new Vector3(dir.x - origin.position.x + origin.localPosition.x, 
                                        dir.y - origin.position.y + origin.localPosition.y, 
                                        dir.z - origin.position.z + origin.localPosition.z);

    }

    protected override float getCooldownTime() 
    {
        return baseCooldown / frame.attackSpeed;
    }
}