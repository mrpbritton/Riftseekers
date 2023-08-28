using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAttack : Attack {
    //  set to -1 for infinite
    [SerializeField] float range = -1f;


    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            attack();
        }
    }

    public override attackType getAttackType() {
        return attackType.Shoot;
    }

    public override int getDamage() {
        return 12;
    }

    public override void attack() {
        //  shoots a raycast FROM THE POSITION OF THE GUN (not from the player's position or the barrel of the gun) in the direction of the cursor
        var dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        RaycastHit hit;
        var thing = Physics.Raycast(transform.position, dir, out hit, range == -1 ? Mathf.Infinity : range, LayerMask.GetMask("Enemy"));
        // Does the ray intersect any objects excluding the player layer
        if(thing) {
            hit.collider.gameObject.GetComponent<Health>().takeDamage(getDamage());
            Debug.Log("here");
        }

        Debug.DrawRay(transform.position, dir);

        /*
        var hit = Physics2D.Raycast(transform.position, dir, range == -1 ? Mathf.Infinity : range, LayerMask.GetMask("Enemy"));

        if(hit.collider != null) {
            hit.collider.gameObject.GetComponent<Health>().takeDamage(getDamage());
        }*/
    }

}
