using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawLine : Attack
{
    public GameObject go;
    private readonly int posCount = 2;
    private LineRenderer lr;

    public override AttackType AType => throw new System.NotImplementedException();
    public override AttackScript AScript => throw new System.NotImplementedException();

    public override void DoAttack() 
    {
        Vector3 point = GetPoint();
        if(lr == null)
        {
            lr = GetComponent<LineRenderer>();
            lr.positionCount = posCount;
        }

        lr.SetPosition(0, gameObject.transform.position);
        lr.SetPosition(lr.positionCount-1, new Vector3(point.x, gameObject.transform.position.y, point.z));
    }
    public override void Anim(Animator anim, bool reset)
    {
    }
    public override void ResetAttack()
    {
    }
    protected override float SetDamage => 3f;
    protected override float SetCooldownTime => 3f;   //  NOTE: this does nothing atm
}
