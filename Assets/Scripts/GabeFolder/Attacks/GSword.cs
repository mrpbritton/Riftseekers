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

    public override void attack()
    {
        Vector3 dir;
        if (isController)
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
            dir = Attack.GetPoint();
            direction = new Vector3(dir.x - origin.position.x + origin.localPosition.x,
                                            dir.y - origin.position.y + origin.localPosition.y,
                                            dir.z - origin.position.z + origin.localPosition.z);
        }
        origin.forward = direction;

        if (damScript.damage != damage)
        {
            damScript.damage = damage;
        }

        cooldownBar.updateSlider(getCooldownTime());
        StartCoroutine(Swing());
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
