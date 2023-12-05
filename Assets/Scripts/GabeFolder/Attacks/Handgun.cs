using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handgun : Attack 
{
    [SerializeField, Tooltip("Used in a calculation to see how much damage dealt")]
    float damage = 1f;
    [SerializeField, Tooltip("Used in a calculation to see how long the cooldown is in seconds")]
    float baseCooldown = 0.4f;
    [SerializeField, Tooltip("Where the bullet instantiates")]
    private Transform origin;
    [SerializeField, Tooltip("Bullet that gets spawned")]
    private GameObject bullet;
    [SerializeField, Tooltip("Time in seconds it takes for bullet to die")]
    private float lifetime;
    private Vector3 cachedDir = new(1, 1, 1);
    protected override void Start()
    {
        base.Start();
        origin = GameObject.FindWithTag("GunOrigin").transform;
        bullet = FindFirstObjectByType<Bullet>(FindObjectsInactive.Include).gameObject;
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
        AkSoundEngine.PostEvent("Pistol_Fire_player", gameObject);

        Vector3 dir;
        Vector3 direction;

        //cooldownBar.updateSlider(getCooldownTime());

        if(!InputManager.isUsingKeyboard())
        {
            dir = new Vector3(pInput.Player.ControllerAim.ReadValue<Vector2>().x, 0, pInput.Player.ControllerAim.ReadValue<Vector2>().y);

            if (dir == Vector3.zero)
            {
                dir = cachedDir;
            }
            else
            {
                cachedDir = dir;
            }

            direction = dir;
        }
        else
        {
            dir = GetPoint();
            direction = new Vector3(dir.x - origin.position.x + origin.localPosition.x,
                                            dir.y - origin.position.y + origin.localPosition.y,
                                            dir.z - origin.position.z + origin.localPosition.z);
        }

        GameObject b = Instantiate(bullet, origin.position, bullet.transform.rotation);
        b.SetActive(true);
        Bullet bs = b.GetComponent<Bullet>(); //(b)ullet(s)cript
        bs.direction = direction;
        b.GetComponent<DoDamage>().damage = getDamage();
        bs.lifetime = lifetime;

    }

    public override void reset()
    {
    }

    protected override float getCooldownTime() 
    {
        return baseCooldown / frame.attackSpeed;
    }
}