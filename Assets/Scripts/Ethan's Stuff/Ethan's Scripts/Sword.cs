using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Attack
{
    // Start is called before the first frame update
    [SerializeField] float damage = 12;
    [SerializeField] GameObject hurtBox, aimRotation;
    [SerializeField] float swingSpeed;
    private float angle = 0;
    private bool slice, direction; //If the attack is happening
    public Collider attacking;
    new private void Start()
    {
        attacking.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(!slice)
                attack();
        }
        if (slice)
        {
            if (!direction)
            {
                if (angle <= 180)
                {
                    angle += Time.deltaTime * swingSpeed * 100;
                    hurtBox.transform.eulerAngles += new Vector3(0, Time.deltaTime * swingSpeed * 100, 0);

                }
                else
                {
                    angle = 180;
                    hurtBox.transform.eulerAngles = new Vector3(0, aimRotation.transform.eulerAngles.y + 180, 0);
                    direction = true;
                    slice = false;
                }
            }
            else
            {
                if (angle >= 0)
                {
                    angle -= Time.deltaTime * swingSpeed * 100;
                    hurtBox.transform.eulerAngles -= new Vector3(0, Time.deltaTime * swingSpeed * 100, 0);
                }
                else
                {
                    angle = 0;
                    hurtBox.transform.eulerAngles = new Vector3(0, aimRotation.transform.eulerAngles.y, 0);
                    direction = false;
                    slice = false;
                    attacking.enabled = false;
                }
            }
        }
    }

    public override void reset()
    {
    }
    public override void anim(Animator anim, bool reset)
    {
    }
    public override attackType getAttackType()
    {
        return attackType.Melee;
    }
    protected override float getDamage()
    {
        return damage;
    }
    public override void attack()
    {
        attacking.enabled = true;
        slice = true;
    }
    protected override float getCooldownTime() {
        return 1f;
    }
}
