using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Attack
{
    [SerializeField, Tooltip("Used in a calculation to see how much damage dealt")]
    float damage = 1f;
    [SerializeField, Tooltip("Used in a calculation to see how long the cooldown is in seconds")]
    float baseCooldown = 3f;
    [SerializeField, Tooltip("Where the bullet instantiates")]
    private Transform origin;
    [SerializeField, Tooltip("Bullet that gets spawned")]
    private GameObject bullet;
    [SerializeField, Tooltip("Time in seconds it takes for bullet to die")]
    private float lifetime;
    [SerializeField, Tooltip("How many bullets are shot")]
    private int bulletCount;
    [SerializeField, Tooltip("How wide the spread is")]
    private float spread;

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
        
        for(int i = 0; i < bulletCount; i++)
        {
            GameObject b = Instantiate(bullet, origin.position, bullet.transform.rotation);
            b.SetActive(true);
            float zRand = Random.Range(-spread / 2, spread / 2);
            float xRand = Random.Range(-spread / 2, spread / 2);
            Vector3 newDir = new Vector3(direction.x + xRand, direction.y, direction.z + zRand);
            //Debug.Log(newDir);
            b.GetComponent<Bullet>().direction = newDir;
            b.GetComponent<Bullet>().damage = getDamage();
            b.GetComponent<Bullet>().lifetime = lifetime;
        }
    }

    protected override float getCooldownTime()
    {
        return baseCooldown / frame.attackSpeed;
    }
}
