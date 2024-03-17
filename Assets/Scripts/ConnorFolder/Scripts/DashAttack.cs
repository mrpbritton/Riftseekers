using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : Attack 
{

    PlayerMovement pm;
    //Coroutine attacker = null;

    //  the radius of the sphere collider that is used to check for collided enemies
    [SerializeField] float radius;
    public override AttackType AType => throw new System.NotImplementedException();
    public override AttackScript AScript => throw new System.NotImplementedException();

    new private void Start() {
        pm = FindObjectOfType<PlayerMovement>();
        if(transform.parent != pm.gameObject.transform)
            transform.parent = pm.gameObject.transform;
    }

    public override void DoAttack() {
        //attacker = StartCoroutine(checkForHits());
    }

    public override void ResetAttack()
    {
    }

    public override void Anim(Animator anim, bool reset)
    {
    }

    protected override float SetDamage => 10f;

    protected override float SetCooldownTime => 1f;
    

/*    IEnumerator checkForHits() {
        float s = Time.time, e = Time.time;
        while(t > 0f) {
            e = Time.time;
            t -= e - s;
            s = Time.time;
            var col =  Physics.OverlapSphere(pm.transform.position, radius, LayerMask.GetMask("Enemy"));
            //  checks for monsters hit
            if(col.Length > 0) {
                foreach(var i in col)
                    i.gameObject.GetComponent<EnemyHealth>().damageTaken(etDamage(), pm.transform.position);
            }
            yield return new WaitForEndOfFrame();
        }

        attacker = null;
    }*/
}
