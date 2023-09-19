using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAttack : Attack {
    //  set to -1 for infinite
    [SerializeField] float range = -1f;
    [SerializeField] float damage = 12;

    [SerializeField] Transform rotTrans;


    public override attackType getAttackType() {
        return attackType.Secondary;
    }

    protected override float getDamage() {
        return damage;
    }

    public override void attack() {
        //  shoots a raycast FROM THE POSITION OF THE GUN (not from the player's position or the barrel of the gun) in the direction of the cursor
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if(Physics.Raycast(transform.position, rotTrans.right, out hit, range == -1 ? Mathf.Infinity : range, LayerMask.GetMask("Enemy"))) {
            hit.collider.gameObject.GetComponent<EnemyHealth>().damageTaken(getDamage());
        }
    }

    protected override float getCooldownTime() {
        return 1f;
    }

}
