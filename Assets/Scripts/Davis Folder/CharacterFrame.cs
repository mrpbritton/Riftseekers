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
    dashCharges,
    attackDamage,
    attackSpeed,
    cooldownMod,
    chargeLimit
}
public enum AttackType
{
    handgun, shotgun, laser
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
    private Health health_s;

    [Tooltip("How fast the player moves")]
    public float movementSpeed;
    [Tooltip("Speed of the dash")]
    public float dashSpeed;
    [Tooltip("How far the dash goes")]
    public float dashDistance;
    [Tooltip("How many charges the dash has")]
    public int dashCharges;
    private PlayerMovement move_s;

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

    [Header("Sprite")]
    public SpriteRenderer characterSprite;

    Coroutine attacker = null;
    bool bIsPressed;

    //more options to come in the future
    private void Start()
    {
        pInput = new PInput();
        pInput.Enable();

        health_s = GetComponent<Health>();
        move_s = GetComponent<PlayerMovement>();

        #region Started Subscriptions
        pInput.Player.BasicAttack.started += ctx => performAttack(basicAttack);
        pInput.Player.SecondAttack.started += ctx => performAttack(secondAttack);
        pInput.Player.Ability1.started += ctx => performAttack(qAbility);
        pInput.Player.Ability2.started += ctx => performAttack(eAbility);
        pInput.Player.Ability3.started += ctx => performAttack(rAbility);
        pInput.Player.Ult.started += ctx => performAttack(fAbility);
        #endregion

        #region Canceled Subscriptions
        pInput.Player.BasicAttack.canceled += ctx => NotPressed();
        pInput.Player.SecondAttack.canceled += ctx => NotPressed();
        pInput.Player.Ability1.canceled += ctx => NotPressed();
        pInput.Player.Ability2.canceled += ctx => NotPressed();
        pInput.Player.Ability3.canceled += ctx => NotPressed();
        pInput.Player.Ult.canceled += ctx => NotPressed();
        #endregion

        AddScript_GA.ChangeAttackType += ReplaceAttack;
    }

    //  waits for the attack cooldown to finish
    IEnumerator attackWaiter(Attack curAttack) 
    {
        do{
            curAttack.attack();
            yield return new WaitForSeconds(curAttack.getRealCooldownTime());
        } while (bIsPressed);
        attacker = null;
    }
    private void ReplaceAttack(AttackType aType)
    {
        bIsPressed = false;
        Destroy(secondAttack);
        switch(aType)
        {
            case AttackType.handgun:
                gameObject.AddComponent<Basic_Proj>();
                secondAttack = GetComponent<Basic_Proj>();
                break;
            case AttackType.shotgun:
                gameObject.AddComponent<Shotgun>();
                secondAttack = GetComponent<Shotgun>();
                break;
            default:
                Debug.LogError("Attack could not be replaced.");
                break;
        }
    }
    void performAttack(Attack curAttack) 
    {
        if(attacker != null) return;
        IsPressed();
            attacker = StartCoroutine(attackWaiter(curAttack));
    }

    void IsPressed()
    {
        bIsPressed = true;
    }

    void NotPressed()
    {
        bIsPressed = false;
    }

    private void OnDisable()
    {
        pInput.Disable();
        StopAllCoroutines();
        AddScript_GA.ChangeAttackType -= ReplaceAttack;
    }
    //tree to execute each respective attack

    public void UpdateStats()
    {
        move_s.UpdateStats();
        health_s.UpdateStats();
    }

    public void UpdateStats(Attack atk)
    {
        atk.updateStats(attackDamage, cooldownMod);
    }
}