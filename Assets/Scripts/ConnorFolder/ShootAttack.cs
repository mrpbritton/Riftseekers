using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAttack : Attack {
    //  set to -1 for infinite
    [SerializeField] float range = -1f;
    [SerializeField] int damage = 12;


    public override attackType getAttackType() {
        return attackType.Secondary;
    }

    public override int getDamage() {
        return damage;
    }

    public override void attack() {
        Debug.Log("shot");
        //  shoots a raycast FROM THE POSITION OF THE GUN (not from the player's position or the barrel of the gun) in the direction of the cursor
        var dir = (Vector3)(Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if(Physics.Raycast(transform.position, dir, out hit, range == -1 ? Mathf.Infinity : range, LayerMask.GetMask("Enemy"))) {
            hit.collider.gameObject.GetComponent<Health>().takeDamage(getDamage());
            //Debug.Log("enemy hit");
        }

        /* for 2D stuff if we have 2D stuff later
        var hit = Physics2D.Raycast(transform.position, dir, range == -1 ? Mathf.Infinity : range, LayerMask.GetMask("Enemy"));

        if(hit.collider != null) {
            hit.collider.gameObject.GetComponent<Health>().takeDamage(getDamage());
        }*/
    }

}
