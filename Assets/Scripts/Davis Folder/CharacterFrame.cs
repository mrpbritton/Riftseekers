using System;
using System.Collections;
using UnityEngine;

/***********************************************************************
 * NOTE: if you add any stats in the enum, it should be added into the 
 * Character frame and vice versa. Additionally, you should update the
 * "UpdateStat_GA" and add a case to the switch with the stat you added.
 ***********************************************************************/

public enum CharStats
{
    maxHealth,
    health,
    moveSpeed,
    dashSpeed,
    dashDistance,
    attackDamage,
    attackSpeed,
    cooldownMod,
    chargeLimit
}

public class CharacterFrame : MonoBehaviour
{
    private PInput pInput;
    [Header("Attacks and Abilities")]
    public Attack basicAttack;
    public Attack secondAttack;
    public Attack qAbility;
    public Attack eAbility;
    public Attack rAbility;
    public Attack fAbility;

    [Header("Stats")]
    [Tooltip("Maximum health of the player")]
    public int maxHealth;
    [Tooltip("Current health of the player")]
    public float health;
    [Tooltip("How fast the player moves")]
    public float movementSpeed;
    [Tooltip("Speed of the dash")]
    public float dashSpeed;
    [Tooltip("How far the dash goes")]
    public float dashDistance;
    [Tooltip("How many charges the dash has")]
    public int dashCharges;
    [Tooltip("Base attack damage; each attack derives this for a calculation")]
    public float attackDamage;
    [Tooltip("Base attack speed; each attack derives this for a calculation")]
    public float attackSpeed;
    [Tooltip("Base cooldown modifier; each ability uses this for a calculation")]
    public float cooldownMod;
    [Tooltip("Limit the ultimate ability will charge to")]
    public int chargeLimit;

    Coroutine attacker = null;

    public static Action UpdateStats = delegate { };

    //more options to come in the future
    private void Start()
    {
        pInput = new PInput();
        pInput.Enable();

        pInput.Player.BasicAttack.started += ctx => performAttack(basicAttack);
        pInput.Player.SecondAttack.started += ctx => performAttack(secondAttack);
        pInput.Player.Ability1.started += ctx => performAttack(qAbility);
        pInput.Player.Ability2.started += ctx => performAttack(eAbility);
        pInput.Player.Ability3.started += ctx => performAttack(rAbility);
        pInput.Player.Ult.started += ctx => performAttack(fAbility);
        UpdateStats();
    }

    //  waits for the attack cooldown to finish
    IEnumerator attackWaiter(float cooldownTime) {
        yield return new WaitForSeconds(cooldownTime);
        attacker = null;
    }

    void performAttack(Attack curAttack) {
        if(attacker != null) return;
        curAttack.attack();
        attacker = StartCoroutine(attackWaiter(curAttack.cooldownTime()));
    }

    /// <summary>
    /// Update the stats of the character frame.
    /// </summary>
    public static void Restat()
    {
        UpdateStats();
    }
    
    private void OnDisable()
    {
        pInput.Disable();
    }
    //tree to execute each respective attack
}