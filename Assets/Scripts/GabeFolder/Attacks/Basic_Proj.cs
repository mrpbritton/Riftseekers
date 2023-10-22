using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Proj : Attack 
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
        Vector3 dir;
        if (isController)
        {
            dir = new Vector3 (pInput.Player.ControllerAim.ReadValue<Vector2>().x,0, pInput.Player.ControllerAim.ReadValue<Vector2>().y);
        }
        else
        {
            dir = Attack.GetPoint();
        }
       /* Vector3 direction = new Vector3(dir.x - origin.position.x + origin.localPosition.x, 
                                        dir.y - origin.position.y + origin.localPosition.y, 
                                        dir.z - origin.position.z + origin.localPosition.z);*/
        GameObject b = Instantiate(bullet, origin.position, bullet.transform.rotation);
        b.SetActive(true);
        b.GetComponent<Bullet>().direction = dir;
        b.GetComponent<Bullet>().damage = getDamage();
        b.GetComponent<Bullet>().lifetime = lifetime;

        AkSoundEngine.PostEvent("Pistol_Fire_player", gameObject);
    }

    protected override float getCooldownTime() 
    {
        return baseCooldown / frame.attackSpeed;
    }
}