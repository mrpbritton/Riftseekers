using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handgun : Attack 
{
    [SerializeField, Tooltip("Used in a calculation to see how much damage dealt")]
    float damage = 2f;
    [SerializeField, Tooltip("Used in a calculation to see how long the cooldown is in seconds")]
    float baseCooldown = 0.4f;
    [SerializeField, Tooltip("Where the bullet instantiates")]
    private Transform origin;
    [SerializeField, Tooltip("Bullet that gets spawned")]
    private GameObject bullet;
    [SerializeField, Tooltip("Time in seconds it takes for bullet to die")]
    private float lifetime;
    private Vector3 cachedDir = new(1, 1, 1);
    [SerializeField, Tooltip("How many times a bullet will pierce an enemy")]
    private int pierceCount;
    public override AttackType AType => AttackType.Ranged;
    public override AttackScript AScript => AttackScript.Handgun;

    protected override void Start()
    {
        base.Start();
        DOTween.Init();
        origin = GameObject.FindWithTag("GunOrigin").transform;
        bullet = FindFirstObjectByType<Bullet>(FindObjectsInactive.Include).gameObject;
        cachedDir = origin.forward;
    }

    protected override float getDamage() 
    {
        return damage * PlayerStats.AttackDamage;
    }

    public override void anim(Animator anim, bool reset)
    {
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
                                            dir.y,
                                            dir.z - origin.position.z + origin.localPosition.z);
        }


        float lungeAmt = -2f;
        //transform.DOComplete();
        FindObjectOfType<PlayerMovement>().slide(direction, lungeAmt, .25f);

        GameObject b = Instantiate(bullet, origin.position, bullet.transform.rotation);
        b.SetActive(true);
        Bullet bs = b.GetComponent<Bullet>(); //(b)ullet(s)cript
        bs.direction = direction;
        bs.bCanPierce = true;
        bs.pierceCount = pierceCount;
        b.GetComponent<DoDamage>().damage = getDamage();
        bs.lifetime = lifetime;
    }

    public override void reset()
    {
    }

    protected override float getCooldownTime() 
    {
        return baseCooldown / PlayerStats.AttackSpeed;
    }
}