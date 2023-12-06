using DG.Tweening;
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
    private Vector3 cachedDir; //filtered direction; cannot be zero
    private CardinalDirection cardDir;
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

    public override void anim()
    {
        
    }

    public override void attack()
    {
        Vector3 dir;
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
                                            dir.y - origin.position.y + origin.localPosition.y,
                                            dir.z - origin.position.z + origin.localPosition.z);
        }
        origin.forward = direction;
        //Debug.Log(origin.forward);
        float angle = Mathf.Atan2(origin.forward.z, origin.forward.x) * Mathf.Rad2Deg;
        origin.forward = constrictDirection(angle);
        //Debug.Log(Mathf.Atan2(origin.forward.z, origin.forward.x) * Mathf.Rad2Deg);

        if (damScript.damage != damage)
        {
            damScript.damage = damage;
        }

        cooldownBar.updateSlider(getCooldownTime());
        StartCoroutine(Swing());
    }

    private Vector3 constrictDirection(float angle)
    {
        Vector3 returnVal = Vector3.zero;

        if(angle < 22.5 && angle > -22.5) //East
        {
            returnVal = new Vector3(1, 0, 0);
            cardDir = CardinalDirection.east;
        }
        else if(angle < 67.5 && angle > 22.5) //NE
        {
            //instead of doing exact math, im rounding sqrt(2)/2
            returnVal = new Vector3(0.7071f, 0, 0.7071f);
            cardDir = CardinalDirection.northEast;
        }
        else if (angle < 112.5 && angle > 67.5) //North
        {
            returnVal = new Vector3(0, 0, 1);
            cardDir = CardinalDirection.north;
        }
        else if (angle < 157.5 && angle > 112.5) //NW
        {
            //instead of doing exact math, im rounding sqrt(2)/2
            returnVal = new Vector3(-0.7071f, 0, 0.7071f);
            cardDir = CardinalDirection.northWest;
        }
        else if (angle < -157.5 || angle > 157.5) //West
        {
            returnVal = new Vector3(-1, 0, 0);
            cardDir = CardinalDirection.west;
        }
        else if (angle < -112.5 && angle > -157.5) //SW
        {
            //instead of doing exact math, im rounding sqrt(2)/2
            returnVal = new Vector3(-0.7071f, 0, -0.7071f);
            cardDir = CardinalDirection.southWest;
        }
        else if(angle < -67.5 && angle > -112.5) //South
        {
            returnVal = new Vector3(0, 0, -1);
            cardDir = CardinalDirection.south;
        }
        else if(angle < -22.5 && angle > -67.5) //SE
        {
            //instead of doing exact math, im rounding sqrt(2)/2
            returnVal = new Vector3(0.7071f, 0, -0.7071f);
            cardDir = CardinalDirection.southEast;
        }
        else //default, East
        {
            returnVal = new Vector3(1, 0, 0);
            cardDir = CardinalDirection.east;
        }

        //Debug.Log(cardDir.ToString());
        
        return returnVal;
    }

    IEnumerator Swing()
    {
        AkSoundEngine.PostEvent("Sword_Swing", gameObject);

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
