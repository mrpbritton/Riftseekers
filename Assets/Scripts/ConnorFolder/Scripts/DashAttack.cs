using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : Attack {

    PlayerMovement pm;
    Coroutine attacker = null;

    //  the radius of the sphere collider that is used to check for collided enemies
    [SerializeField] float radius;

    new private void Start() {
        pm = FindObjectOfType<PlayerMovement>();
        if(transform.parent != pm.gameObject.transform)
            transform.parent = pm.gameObject.transform;
    }

    public override void attack() {
        StartCoroutine(pm.Dash());
        attacker = StartCoroutine(checkForHits());
    }

    public override void reset()
    {
    }

    public override void anim(Animator anim, bool reset)
    {
    }
    public override attackType getAttackType() {
        return attackType.EAbility;
    }

    protected override float getDamage() {
        return 10f;
    }

    protected override float getCooldownTime() {
        return 1f;
    }

    IEnumerator checkForHits() {
        float t = pm.getDashTime();
        float s = Time.time, e = Time.time;
        while(t > 0f) {
            e = Time.time;
            t -= e - s;
            s = Time.time;
            var col =  Physics.OverlapSphere(pm.transform.position, radius, LayerMask.GetMask("Enemy"));
            //  checks for monsters hit
            if(col.Length > 0) {
                foreach(var i in col)
                    i.gameObject.GetComponent<EnemyHealth>().damageTaken(getDamage(), pm.transform.position);
            }
            yield return new WaitForEndOfFrame();
        }

        attacker = null;
    }
}
