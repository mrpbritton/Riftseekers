using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSword : Attack
{
    [SerializeField, Tooltip("Used in a calculation to see how much damage dealt")]
    float damage = 1f;
    [SerializeField, Tooltip("Used in a calculation to see how long the cooldown is in seconds")]
    float baseCooldown = .5f;
    [SerializeField, Tooltip("How long the hitbox stays")]
    float hitboxTime = .05f;
    [SerializeField, Tooltip("The axis that rotates according to the player's movement")]
    private Transform origin;
    [SerializeField, Tooltip("Hitbox the Sword uses")]
    private Transform hitbox;
    private DoDamage damScript;
    private Vector3 direction; //what the direction currently is
    private Vector3 cachedDirection; //filtered direction; cannot be zero

    protected override void Start()
    {
        base.Start();

        damScript = hitbox.GetComponent<DoDamage>();
        damScript.damage = damage;
    }
    public override attackType getAttackType()
    {
        return attackType.Secondary;
    }

    protected override float getDamage()
    {
        return damage * frame.attackDamage;
    }

    private void Update()
    { 
        direction.x = pInput.Player.Movement.ReadValue<Vector3>().x;
        direction.z = pInput.Player.Movement.ReadValue<Vector3>().z;

        direction = direction.normalized;

        if(direction != Vector3.zero && direction != cachedDirection)
        {
            cachedDirection = direction;
        }
    }

    public override void attack()
    {
        #region Rotation Setting
        if (direction.x > 0)
        {
            if (direction.z < 0) // SOUTHEAST
            {
                origin.rotation = Quaternion.Euler(new Vector3(0, 315, 0));
            }
            else if (direction.z == 0) // EAST
            {
                origin.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
            }
            else // direction.z == 1 *** NORTHEAST
            {
                origin.rotation = Quaternion.Euler(new Vector3(0, 225, 0));
            }
        }
        else if (direction.x < 0)
        {
            if (direction.z < 0) // SOUTHWEST
            {
                origin.rotation = Quaternion.Euler(new Vector3(0, 45, 0));
            }
            else if (direction.z == 0) // WEST
            {
                origin.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }
            else // direction.z == 1 *** NORTHWEST
            {
                origin.rotation = Quaternion.Euler(new Vector3(0, 135, 0));
            }
        }
        else //direction.x == 0
        {
            if (direction.z < 0) // SOUTH
            {
                origin.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else if (direction.z == 0) // NO INPUT
            {
                //last input entered
            }
            else // direction.z == 1 *** NORTH
            {
                origin.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
        }
        #endregion
        
        if (damScript.damage != damage)
        {
            damScript.damage = damage;
        }
        cooldownBar.updateSlider(getCooldownTime());
        StartCoroutine(Swing());
    }

    IEnumerator Swing()
    {
        hitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(hitboxTime);
        hitbox.gameObject.SetActive(false);
    }

    public override void reset()
    {
        if(hitbox.gameObject.activeSelf == true)
            hitbox.gameObject.SetActive(false);
    }


    protected override float getCooldownTime()
    {
        return baseCooldown / frame.attackSpeed;
    }
}
