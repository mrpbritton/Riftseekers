using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CharacterFrame))]
public class PlayerMovement : MonoBehaviour
{
    public PInput pInput;

    [Header("Regular Movement")]
    [SerializeField, Tooltip("How fast the player moves")]
    private float speed;
    private Vector3 direction;
    [SerializeField, Tooltip("How fast the player falls")]
    private float fallSpeed;

    [Header("Dash")]
    [SerializeField, Tooltip("Charges the dash has. Must be at least one.")]
    private int dashCharges;
    [SerializeField]
    private int remainingCharges; //how many charges the dash has left
    [SerializeField, Tooltip("Time it takes for a whole dash to recharge in seconds")]
    private float dashChargeCooldown;
    private bool canRecharge; //if the dash can recharge
    [SerializeField, Tooltip("Time between keypresses of the dash ability in seconds")]
    private float dashCooldown;
    [SerializeField, Tooltip("Distance modifier of the dash. Will multiply against the speed. Must be at least one.")]
    private float dashDistance;
    [SerializeField, Tooltip("How long the dash takes in seconds")]
    private float dashTime;
    [SerializeField, Tooltip("Speed of the dash")]
    private float dashSpeed;
    private bool cantDash; //whether or not the player can dash

    private CharacterController player;
    private CharacterFrame frame;
    private Animator character;
    private Vector3 cachedDirection;
    private Vector3 dashDirection;
    public static Transform playerTrans;
    private bool bMove = true;
    //<--- Click on the plus sign to expand
    #region Setup
    private void Awake()
    {
        bonuspicker.EnablePlayerMovement += EnablePlayerMovement;
        bonuspicker.DisablePlayerMovement += DisablePlayerMovement;
    }
    private void OnEnable()
    {        
        playerTrans = transform;
        pInput = new PInput();
        pInput.Enable();
        player = gameObject.GetComponent<CharacterController>();
        frame = gameObject.GetComponent<CharacterFrame>();
        character = gameObject.GetComponentInChildren<Animator>();
        speed = frame.movementSpeed;
        dashSpeed = frame.dashSpeed;
        pInput.Player.Dash.started += DashPress;

        canRecharge = true;
        UpdateStats();
        remainingCharges = frame.dashCharges;
    }
    private void OnDisable()
    {
        pInput.Disable();
        pInput.Player.Dash.started -= DashPress;
        bonuspicker.EnablePlayerMovement += EnablePlayerMovement;
        bonuspicker.DisablePlayerMovement += DisablePlayerMovement;
    }
    #endregion

    
    public void UpdateStats()
    {
        speed = frame.movementSpeed;
        dashSpeed = frame.dashSpeed;
        dashDistance = frame.dashDistance;
        dashCharges = frame.dashCharges;
        dashCooldown *= frame.cooldownMod;
        dashChargeCooldown *= frame.cooldownMod;
    }
    private void EnablePlayerMovement()
    {
        bMove = true; 
    }
    private void DisablePlayerMovement() 
    { 
        bMove = false;
    }
    private void Update()
    {
        if (bMove)
        {
            direction.x = pInput.Player.Movement.ReadValue<Vector3>().x;
            direction.z = pInput.Player.Movement.ReadValue<Vector3>().z;

            direction = direction.normalized;

            if (cachedDirection != direction)
            {
                cachedDirection = direction;
                frame.UpdateSprite(cachedDirection);
            }

            player.Move(speed * Time.deltaTime * direction);

            //if the player isn't grounded, move them towards the ground.
            if (player.isGrounded == false)
                if (player.isGrounded == false)
                {
                    player.Move(fallSpeed * Time.deltaTime * Vector3.down);
                }

            if (remainingCharges < dashCharges && canRecharge)
            {
                StartCoroutine(RechargeDash());
            }
        }
    }

    //<--- Click on the plus sign to expand
    #region Dash
    private void DashPress(InputAction.CallbackContext c)
    {
        if (cantDash || remainingCharges == 0) return;
        StartCoroutine(Dash());
    }
    //cbt otherwise known as cock and ball torture

    public IEnumerator Dash()
    {
        AkSoundEngine.PostEvent("Dash", gameObject);

        cantDash = true;
        bool isDefDash = false; //is default dash
        float dTimeRemaining = dashTime;

        dashDirection = direction;

        #region Sprite Setting
        if (direction.x > 0)
        {

            if (direction.z < 0) // SOUTHEAST
            {
                //characterSprite.sprite = southEast;
                character.SetTrigger("DashSE");
            }
            else if (direction.z == 0) // EAST
            {
                character.SetTrigger("DashE");
            }
            else // direction.z == 1 *** NORTHEAST
            {
                character.SetTrigger("DashNE");
            }
        }
        else if (direction.x < 0)
        {
            if (direction.z < 0) // SOUTHWEST
            {
                //characterSprite.sprite = southWest;
                character.SetTrigger("DashSW");
            }
            else if (direction.z == 0) // WEST
            {
                character.SetTrigger("DashW");
            }
            else // direction.z == 1 *** NORTHWEST
            {
                //characterSprite.sprite = northWest;
                character.SetTrigger("DashNW");
            }
        }
        else //direction.x == 0
        {
            if (direction.z < 0) // SOUTH
            {
                character.SetTrigger("DashS");
            }
            else if (direction.z == 0) // NO INPUT
            {
                //EAST BY DEFAULT
                character.SetTrigger("DashDef");
                dashDirection = Vector3.right;
                isDefDash = true;
            }
            else // direction.z == 1 *** NORTH
            {
                character.SetTrigger("DashN");
            }
        }
        #endregion
        /* The loop below ends up being a pseudo-update function. This is able to 
         * happen because of the yield return null; at the end of this while loop.
         * Every iteration makes the dTimeRemaining decrease until it is zero (or 
         * until the dash is completed), while the position of the player continues 
         * to move towards the direction */

        while (dTimeRemaining > 0)
        {
            dTimeRemaining -= Time.deltaTime;
            player.Move(dashDistance * dashSpeed * Time.deltaTime * dashDirection.normalized);
            yield return null;
        }

        yield return new WaitForSeconds(dashCooldown);
        remainingCharges--; //since dash was performed, subtract a dash
        cantDash = false;
        if(isDefDash)
        {
            character.SetTrigger("WalkE");
            frame.UpdateSprite(CardinalDirection.east);
        }

        frame.UpdateSprite(dashDirection);
    }

    public IEnumerator RechargeDash()
    {
        canRecharge = false;
        yield return new WaitForSeconds(dashChargeCooldown);
        remainingCharges = dashCharges;
        canRecharge = true;
    }

    //  for connor's dash attack
    public float getDashTime()
    {
        return dashTime;
    }
    #endregion
}