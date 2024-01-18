using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Tooltip("The stats that the player will start out with")]
    public DefaultStats_SO startingStats;
    private static float health, movementSpeed, dashSpeed, dashDistance, attackDamage, attackSpeed, cooldownMod, charge;
    private static int maxHealth, dashCharges, chargeLimit;

    /*** "READ-ONLY" FIELDS ***/
    /*  These fields are designed to be accessed, but not changed. Whenever you need
     *  to access the values of a player in any way, use these. These are connected to
     *  the actual scripts that touch them.
     */
    public static float Health => health;
    public static int   MaxHealth => maxHealth;
    public static float MovementSpeed => movementSpeed;
    public static float DashSpeed => dashSpeed;
    public static float DashDistance => dashDistance;
    public static int   DashCharges => dashCharges;
    public static float AttackDamage => attackDamage;
    public static float AttackSpeed => attackSpeed;
    public static float CooldownMod => cooldownMod;
    public static float Charge => charge;
    public static int   ChargeLimit => chargeLimit;

    /*** PROPERTIES ***/
    /* Very few things should touch these properties. Because of that, a Debug.Log
     * statement is inside of each of these, which should hopefully show which script
     * is accessing each value so then we can determine why.
     */
    public static float UpdateHealth
    {
        get { return health; }

        set
        {
            Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            health = value;
        }

    }
    public static int UpdateMaxHealth
    {
        get { return maxHealth; }

        set
        {
            Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            movementSpeed = value;
        }

    }
    public static float UpdateMovementSpeed
    {
        get { return movementSpeed; }

        set
        {
            Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            movementSpeed = value;
        }

    }
    public static float UpdateDashSpeed
    {
        get { return dashSpeed; }

        set
        {
            Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            dashSpeed = value;
        }

    }
    public static float UpdateDashDistance
    {
        get { return dashDistance; }

        set
        {
            Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            dashDistance = value;
        }

    }
    public static int UpdateDashCharges
    {
        get { return dashCharges; }

        set
        {
            Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            dashCharges = value;
        }

    }
    public static float UpdateAttackDamage
    {
        get { return attackDamage; }

        set
        {
            Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            attackDamage = value;
        }

    }
    public static float UpdateAttackSpeed
    {
        get { return attackSpeed; }

        set
        {
            Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            attackSpeed = value;
        }

    }
    public static float UpdateCooldownMod
    {
        get { return cooldownMod; }

        set
        {
            Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            cooldownMod = value;
        }

    }
    public static float UpdateCharge
    {
        get { return charge; }

        set
        {
            Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            charge = value;
        }

    }
    public static int UpdateChargeLimit
    {
        get { return chargeLimit; }

        set
        {
            Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            chargeLimit = value;
        }

    }

    private void Awake()
    {
        health  = startingStats.health;
        movementSpeed = startingStats.movementSpeed;
        dashSpeed = startingStats.dashSpeed;
        dashDistance = startingStats.dashDistance;
        attackDamage = startingStats.attackDamage;
        attackSpeed = startingStats.attackSpeed;
        cooldownMod = startingStats.cooldownMod;
        charge = startingStats.charge;
        maxHealth = startingStats.maxHealth;
        dashCharges = startingStats.dashCharges;
        chargeLimit = startingStats.chargeLimit;
    }
}
