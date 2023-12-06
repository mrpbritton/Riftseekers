using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class DefaultStats_SO : ScriptableObject
{
    [Header("Attacks and Abilities")]
    public AttackScript basicAttack;
    public AttackScript secondAttack;
    public AttackScript qAbility;
    public AttackScript eAbility;
    public AttackScript rAbility;
    public AttackScript fAbility;

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
    [Tooltip("Current charge of the ultimate")]
    public float charge;

}
