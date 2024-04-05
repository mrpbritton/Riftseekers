using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStats : MonoBehaviour {
    [Tooltip("The stats that the player will start out with")]
    [SerializeField] DefaultStats_SO startingStats;
    static PlayerStatSaveData stats;

    private static float maxHealth = 20;
    private static float movementSpeed = 10;

    /*** "READ-ONLY" FIELDS ***/
    /*  These fields are designed to be accessed, but not changed. Whenever you need
     *  to access the values of a player in any way, use these. These are connected to
     *  the actual scripts that touch them.
     */
    public static float Health => stats.health;
    public static float MaxHealth => stats.maxHealth;
    public static float MovementSpeed => stats.movementSpeed;
    public static float DashSpeed => stats.dashSpeed;
    public static float DashDistance => stats.dashDistance;
    public static int DashCharges => stats.dashCharges;
    public static float AttackDamage => stats.attackDamage;
    public static float AttackSpeed => stats.attackSpeed;
    public static float CooldownMod => stats.cooldownMod;
    public static float Charge => stats.charge;
    public static int ChargeLimit => stats.chargeLimit;

    const string statsTag = "PlayerStatsTag";

    /*** PROPERTIES ***/
    /* Very few things should touch these properties. Because of that, a Debug.Log
     * statement is inside of each of these, which should hopefully show which script
     * is accessing each value so then we can determine why.
     */
    public static float UpdateHealth {
        get { return stats.health; }

        set {
            stats.health = value;
            stats.health = Mathf.Clamp(stats.health, 0, MaxHealth);
        }

    }
    public static float UpdateMaxHealth {
        get { return stats.maxHealth; }

        set {
            // Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            stats.maxHealth = value;
            stats.maxHealth = Mathf.Clamp(stats.maxHealth, 0, maxHealth*3);
        }

    }
    public static float UpdateMovementSpeed {
        get { return stats.movementSpeed; }

        set {
            //Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            stats.movementSpeed = value;
            stats.movementSpeed = Mathf.Clamp(stats.movementSpeed, 0, movementSpeed * 2);
        }

    }
    public static float UpdateDashSpeed {
        get { return stats.dashSpeed; }

        set {
            //Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            stats.dashSpeed = value;
        }

    }
    public static float UpdateDashDistance {
        get { return stats.dashDistance; }

        set {
            //Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            stats.dashDistance = value;
        }

    }
    public static int UpdateDashCharges {
        get { return stats.dashCharges; }

        set {
            //Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            stats.dashCharges = value;
        }

    }
    public static float UpdateAttackDamage {
        get { return stats.attackDamage; }

        set {
            //Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            stats.attackDamage = value;
        }

    }
    public static float UpdateAttackSpeed {
        get { return stats.attackSpeed; }

        set {
            //Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            stats.attackSpeed = value;
        }

    }
    public static float UpdateCooldownMod {
        get { return stats.cooldownMod; }

        set {
            //Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            stats.cooldownMod = value;
        }

    }
    public static float UpdateCharge {
        get { return stats.charge; }

        set {
            //Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            stats.charge = value;
            Mathf.Clamp(stats.charge, 0, ChargeLimit);
        }

    }
    public static int UpdateChargeLimit {
        get { return stats.chargeLimit; }

        set {
            //Debug.Log("Who the hell is changing this"); //words by Peter Britton, everybody
            stats.chargeLimit = value;
        }

    }

    private void Awake() {
        load();
        save();
    }

    public void save() {
        var data = JsonUtility.ToJson(stats);
        SaveData.setString(statsTag, data);
    }
    public void load() {
        var data = SaveData.getString(statsTag);
        stats = string.IsNullOrEmpty(data) ? new PlayerStatSaveData(startingStats.stats) : JsonUtility.FromJson<PlayerStatSaveData>(data);
    }

    public void getAugmented(Augment_SO.augmentType type) {
        if(type == Augment_SO.augmentType.None) return;
        var aug = AugmentLibrary.I.getAugment(type);

        foreach(var i in aug.mods) {
            singleStatMod(i.stat, i.mod);
        }

        save();
    }

    void singleStatMod(CharStats stat, float mod) {
        if(stat == CharStats.none || mod == 1f) return;
        mod += 1f;  //  because percentage
        switch(stat) {
            case CharStats.maxHealth: UpdateMaxHealth *= mod; break;
            case CharStats.health: UpdateHealth *= mod; break;
            case CharStats.moveSpeed: UpdateMovementSpeed *= mod; break;
            case CharStats.dashSpeed: UpdateDashSpeed *= mod; break;
            case CharStats.dashDistance: UpdateDashDistance *= mod; break;
            case CharStats.dashCharges: UpdateDashCharges = (int)(UpdateDashCharges * mod); break;
            case CharStats.attackDamage: UpdateAttackDamage *= mod; break;
            case CharStats.attackSpeed: UpdateAttackSpeed *= mod; break;
            case CharStats.cooldownMod: UpdateCooldownMod *= mod; break;
            case CharStats.chargeLimit: UpdateChargeLimit = (int)(UpdateChargeLimit * mod); break;
        }
    }
}

[System.Serializable]
public class PlayerStatSaveData {
    public float health;
    public float movementSpeed;
    public float dashSpeed;
    public float dashDistance;
    public float attackDamage;
    public float attackSpeed;
    public float cooldownMod;
    public float charge;
    public float maxHealth;
    public int dashCharges;
    public int chargeLimit;

    public PlayerStatSaveData(PlayerStatSaveData other) {
        health = other.health;
        movementSpeed = other.movementSpeed;
        dashSpeed = other.dashSpeed;
        dashDistance = other.dashDistance;
        attackDamage = other.attackDamage;
        attackSpeed = other.attackSpeed;
        cooldownMod = other.cooldownMod;
        charge = other.charge;
        maxHealth = other.maxHealth;
        dashCharges = other.dashCharges;
        chargeLimit = other.chargeLimit;
    }
}