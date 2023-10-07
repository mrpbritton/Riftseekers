using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CreateAt : Attack
{
    public GameObject go;
    public int posCount;
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
    
        float avgChangeX = (point.x - transform.position.x) / lr.positionCount;
        float avgChangeZ = (point.z - transform.position.z) / lr.positionCount;

        for (int i = 1; i < lr.positionCount-1; i++)
        {
            lr.SetPosition(i, new Vector3(i * avgChangeX, gameObject.transform.position.y, i * avgChangeZ));
        }
    }
    protected override float getDamage() { return 3f; }
    protected override float getCooldownTime() { return 3f; }   //  NOTE: this does nothing atm
}
