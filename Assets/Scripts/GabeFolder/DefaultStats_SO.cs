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

    public PlayerStatSaveData stats;
}
