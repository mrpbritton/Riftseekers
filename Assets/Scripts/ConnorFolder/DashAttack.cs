using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : Attack {

    PlayerMovement pm;

    private void Start() {
        pm = FindObjectOfType<PlayerMovement>();
        if(transform.parent != pm.gameObject.transform)
            transform.parent = pm.gameObject.transform;
    }

    public override void attack() {
        Debug.Log("here");
        if(Physics.CheckSphere(transform.position, 3f, LayerMask.GetMask("Enemy"))) {
            StartCoroutine(pm.Dash());
        }
    }

    public override attackType getAttackType() {
        return attackType.EAbility;
    }

    public override int getDamage() {
        return 15;
    }

    public override float cooldownTime() {
        return 5f;
    }
}
