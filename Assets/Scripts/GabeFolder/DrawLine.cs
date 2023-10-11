using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawLine : Attack
{
    public GameObject go;
    private readonly int posCount = 2;
    private LineRenderer lr;
    public override attackType getAttackType()
    {
        return attackType.Secondary;
    }
    public override void attack() 
    {
        Vector3 point = Attack.GetPoint();
        if(lr == null)
        {
            lr = GetComponent<LineRenderer>();
            lr.positionCount = posCount;
        }

        lr.SetPosition(0, gameObject.transform.position);
        lr.SetPosition(lr.positionCount-1, new Vector3(point.x, gameObject.transform.position.y, point.z));
    }
    protected override float getDamage() { return 3f; }
    protected override float getCooldownTime() { return 3f; }   //  NOTE: this does nothing atm
}
