using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Basic_Shoot : Attack {
    //  set to -1 for infinite
    [SerializeField, Tooltip("How far the bullet's raycast goes")] 
    float range = 10f;
    [SerializeField, Tooltip("Used in a calculation to see how much damage dealt")]
    float damage = 1f;
    [SerializeField, Tooltip("Used in a calculation to see how long the cooldown is in seconds")]
    float baseCooldown = 3f;
    [SerializeField, Tooltip("How fast the bullet trail disappears")]
    float trailSpeed = 3f;
    [SerializeField, Tooltip("Where the raycast comes from")]
    Transform origin;
    LineRenderer line;
    
    bool bCanShoot = true;

    new private void Start()
    {
        //start and end point
        line.positionCount = 2;
        line.enabled = false;
    }

    public override attackType getAttackType() {
        return attackType.Secondary;
    }

    protected override float getDamage() {
        return damage * frame.attackDamage;
    }

    public override void attack()
    {
        Vector3 direction = Attack.GetPoint();
        direction = new Vector3(direction.x, direction.y - origin.position.y + origin.localPosition.y, direction.z );
        
        Debug.DrawRay(origin.position, direction, Color.green, 5);
        if(Physics.Raycast(origin.position, direction, out RaycastHit gotHit, range))
        {
            if (gotHit.collider.gameObject.TryGetComponent(out EnemyHealth enemy) && bCanShoot)
            {
                Debug.Log(true);
                enemy.damageTaken(damage * frame.attackDamage);
                StartCoroutine( DrawLine(origin.position, gotHit.point) );
                StartCoroutine( AttackCooldown() );
            }
            else /*FindObjectOfType(explode)*/
            {
                Debug.Log(false);

            }
        }
        //else: complete miss
    }

    protected override float getCooldownTime() {
        return baseCooldown / frame.attackSpeed;
    }

    /// <summary>
    /// This uses attackSpeed for its cooldown
    /// </summary>
    /// <returns>Stops after a time in seconds</returns>
    private IEnumerator AttackCooldown()
    {
        bCanShoot = false;
        yield return new WaitForSeconds(baseCooldown / frame.attackSpeed);
        line.enabled = false;
        bCanShoot = true;
    }

    private IEnumerator DrawLine(Vector3 origin, Vector3 point)
    {
        line.SetPosition(0, origin);
        line.SetPosition(1, point);
        line.enabled = true;
        yield return new WaitForSeconds(0);
        line.enabled = false;
    }
}
