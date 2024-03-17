using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Attack
{
    // Start is called before the first frame update
    protected override float SetDamage => 12f;
    [SerializeField] GameObject hurtBox, aimRotation;
    [SerializeField] float swingSpeed;
    private float angle = 0;
    private bool slice, direction; //If the attack is happening
    public Collider attacking;
    public override AttackType AType => throw new System.NotImplementedException();
    public override AttackScript AScript => throw new System.NotImplementedException();

    new private void Start()
    {
        attacking.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(!slice)
                DoAttack();
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

    public override void ResetAttack()
    {
    }
    public override void Anim(Animator anim, bool reset)
    {
    }
    public override void DoAttack()
    {
        attacking.enabled = true;
        slice = true;
    }
    protected override float SetCooldownTime => 1f;
}
