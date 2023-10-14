using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Basic_Hitscan : Attack {

    [Header("Gun Attributes")]
    [SerializeField, Tooltip("How far the bullet's raycast goes")] 
    float range = 10f;
    [SerializeField, Tooltip("Used in a calculation to see how much damage dealt")]
    float damage = 1f;
    [SerializeField, Tooltip("Used in a calculation to see how long the cooldown is in seconds")]
    float baseCooldown = 3f;
    [Header("Trail Attributes")]
    [SerializeField, Tooltip("How fast the bullet trail disappears")]
    float trailSpeed = 0.05f;
    [SerializeField, Tooltip("Where the raycast comes from")]
    Transform origin;
    LineRenderer line;
    
    bool bCanShoot = true;

    new private void Start()
    {
        //start and end point
        base.Start();
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.enabled = false;
    }

    public override attackType getAttackType() 
    {
        return attackType.Secondary;
    }

    protected override float getDamage() 
    {
        return damage * frame.attackDamage;
    }

    public override void attack()
    {
        Vector3 dir = Attack.GetPoint();
        Vector3 direction = new Vector3(dir.x - origin.position.x + origin.localPosition.x, 
                                        dir.y - origin.position.y + origin.localPosition.y, 
                                        dir.z - origin.position.z + origin.localPosition.z);
        /*Debug.Log($"dir: {dir} | direction: {direction}");*/
        Debug.DrawRay(origin.position, direction, Color.green, 5);
        if(Physics.Raycast(origin.position, direction, out RaycastHit gotHit, range) && bCanShoot)
        {
            if (gotHit.collider.gameObject.TryGetComponent(out EnemyHealth enemy))
            {
                enemy.damageTaken(damage * frame.attackDamage);
            }
            else /*FindObjectOfType(explode)*/
            {
                Debug.Log("AAA");
            }
            StartCoroutine(DrawLine(origin.position, gotHit.point));
        }
        else if (bCanShoot)
        {
            StartCoroutine(DrawLine(origin.position, dir));
        }
    }

    protected override float getCooldownTime() 
    {
        return baseCooldown / frame.attackSpeed;
    }


    private IEnumerator DrawLine(Vector3 origin, Vector3 point)
    {
        line.enabled = true;
        line.SetPosition(0, origin);
        line.SetPosition(1, point);
        yield return new WaitForSeconds(trailSpeed);
        line.enabled = false;
    }
}