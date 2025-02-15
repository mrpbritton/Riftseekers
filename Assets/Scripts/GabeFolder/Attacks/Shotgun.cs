using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shotgun : Attack
{
    protected override float SetDamage => 1f;
    protected override float SetCooldownTime => 0.75f;
    [SerializeField, Tooltip("Where the bullet instantiates")]
    private Transform origin;
    [SerializeField, Tooltip("Bullet that gets spawned")]
    private GameObject bullet;
    [SerializeField, Tooltip("Time in seconds it takes for bullet to die")]
    private float lifetime = 3f;
    [SerializeField, Tooltip("How many bullets are shot")]
    private int bulletCount = 4;
    [SerializeField, Tooltip("How wide the spread is")]
    private float spread = 0.35f;
/*    [SerializeField, Tooltip("How strong the knockback is")]
    private float knockback = 1f;*/
    private Vector3 cachedDir = new(1, 1, 1);
    public override AttackType AType => AttackType.Ranged;
    public override AttackScript AScript => AttackScript.Shotgun;

    protected override void Start()
    {
        base.Start();
        pInput.Enable();
        origin = GameObject.FindWithTag("GunOrigin").transform;
        bullet = AttackManager.I.bullet;
        cachedDir = origin.forward;
    }

    public override void Anim(Animator anim, bool reset)
    {
    }

    public override void DoAttack()
    {
        AkSoundEngine.PostEvent("Shotgun_Fire", gameObject);

        Vector3 dir;
        Vector3 direction;
        if (!InputManager.isUsingKeyboard())
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

        direction = direction.normalized;
        
        for(int i = 0; i < bulletCount; i++)
        {
            GameObject b = BulletManager.I.getBullet();
            float zRand = Random.Range(-spread / 2, spread / 2);
            float xRand = Random.Range(-spread / 2, spread / 2);
            Vector3 newDir = new Vector3(direction.x + xRand, direction.y, direction.z + zRand);
            Bullet bScript = b.GetComponent<Bullet>();
            bScript.direction = newDir.normalized;
            bScript.GetComponent<DoDamage>().damage = GetDamage();
            bScript.lifetime = lifetime;
        }

        float lungeAmt = -6f;
        //transform.DOComplete();
        FindObjectOfType<PlayerMovement>().slide(direction, lungeAmt, .25f);

        /*        frame.gameObject.GetComponent<CharacterController>().Move(-direction * knockback);*/
    }

    public override void ResetAttack()
    {
        //
    }
}
