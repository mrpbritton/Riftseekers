using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Shoot : Attack {
    //  set to -1 for infinite
    [SerializeField, Tooltip("How far the bullet's raycast goes")] 
    float range = 10f;
    [SerializeField, Tooltip("Used in a calculation to see how much damage dealt")]
    float damage = 1f;
    [SerializeField, Tooltip("Used in a calculation to see how long the cooldown is in seconds")]
    float baseCooldown = 3f;
    [SerializeField, Tooltip("Where the raycast comes from")]
    Transform origin;
    
    bool bCanShoot = true;

    public override attackType getAttackType() {
        return attackType.Secondary;
    }

    protected override float getDamage() {
        return damage * frame.attackDamage;
    }

    public override void attack()
    {
        Vector3 direction = Attack.GetPoint();
    
        if(Physics.Raycast(origin.position, direction, out RaycastHit gotHit, range))
        {
            if(gotHit.collider.gameObject.TryGetComponent(out EnemyHealth enemy) && bCanShoot)
            {
                enemy.damageTaken(damage * frame.attackDamage);
                
            }
            else /*FindObjectOfType(explode)*/
            {

            }
        }
        //else: complete miss
    }

    protected override float getCooldownTime() {
        return baseCooldown / frame.attackSpeed;
    }

    /// <summary>
    /// This uses attackSpeed for its cooldown
    /// </summary>
    /// <returns>Stops after a time in seconds</returns>
    private IEnumerator AttackCooldown()
    {
        bCanShoot = false;
        yield return new WaitForSeconds(baseCooldown / frame.attackSpeed);
        bCanShoot = true;
    }
}
