using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Attack
{
    // Start is called before the first frame update
    [SerializeField] int damage = 12;
    [SerializeField] GameObject hurtBox;
    [SerializeField] float swingSpeed;
    public float angle = 0;
    public bool slice, direction; //If the attack is happening

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
                    hurtBox.transform.rotation = Quaternion.Euler(hurtBox.transform.rotation.x, angle, hurtBox.transform.rotation.z);
                }
                else
                {
                    angle = 180;
                    hurtBox.transform.rotation = Quaternion.Euler(hurtBox.transform.rotation.x, angle, hurtBox.transform.rotation.z);
                    direction = true;
                    slice = false;
                }
            }
            else
            {
                if (angle >= 0)
                {
                    angle -= Time.deltaTime * swingSpeed * 100;
                    hurtBox.transform.rotation = Quaternion.Euler(hurtBox.transform.rotation.x, angle, hurtBox.transform.rotation.z);
                }
                else
                {
                    angle = 0;
                    hurtBox.transform.rotation = Quaternion.Euler(hurtBox.transform.rotation.x, angle, hurtBox.transform.rotation.z);
                    direction = false;
                    slice = false;
                }
            }
        }
    }
    public override attackType getAttackType()
    {
        return attackType.Sword;
    }
    public override int getDamage()
    {
        return damage;
    }
    public override void attack()
    {
        slice = true;
    }
}
