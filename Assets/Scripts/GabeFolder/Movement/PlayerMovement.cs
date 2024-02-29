using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public PInput pInput;

    //[Header("Regular Movement")]
    [SerializeField, Tooltip("How fast the player moves")]
    private float speed => PlayerStats.MovementSpeed;
    private Vector3 direction;
    [SerializeField, Tooltip("How fast the player falls")]
    private float fallSpeed;

    [SerializeField, Tooltip("Charges the dash has. Must be at least one.")]
    private int dashCharges => PlayerStats.DashCharges;
    [Header("Dash")]
    [SerializeField]
    private int remainingCharges; //how many charges the dash has left
    [SerializeField, Tooltip("Time it takes for a whole dash to recharge in seconds")]
    private float dashChargeCooldown;
    private bool canRecharge; //if the dash can recharge
    [SerializeField, Tooltip("Time between keypresses of the dash ability in seconds")]
    private float dashCooldown;
    [SerializeField, Tooltip("Distance modifier of the dash. Will multiply against the speed. Must be at least one.")]
    private float dashDistance => PlayerStats.DashDistance;
    [SerializeField, Tooltip("How long the dash takes in seconds")]
    private float dashTime;
    [SerializeField, Tooltip("Speed of the dash")]
    private float dashSpeed => PlayerStats.DashSpeed;
    private bool cantDash; //whether or not the player can dash

    Coroutine slider = null;

    private CharacterController player;
    private SpriteManager characterAnim;
    private Vector3 cachedDirection;
    private Vector3 dashDirection;
    public static Transform playerTrans;
    private bool bMove = true;
    private DashUI dashUI;
    //<--- Click on the plus sign to expand
    #region Setup
    private void Awake()
    {
        bonuspicker.EnablePlayerMovement += EnablePlayerMovement;
        bonuspicker.DisablePlayerMovement += DisablePlayerMovement;
    }
    private void OnEnable()
    {
        DashUI.UpdateDashUI(remainingCharges);
        playerTrans = transform;
        pInput = new PInput();
        pInput.Enable();
        player = gameObject.GetComponent<CharacterController>();
        characterAnim = gameObject.GetComponent<SpriteManager>();
        pInput.Player.Dash.started += DashPress;
        canRecharge = true;
    }
    private void OnDisable()
    {
        pInput.Disable();
        pInput.Player.Dash.started -= DashPress;
        bonuspicker.EnablePlayerMovement += EnablePlayerMovement;
        bonuspicker.DisablePlayerMovement += DisablePlayerMovement;
    }
    #endregion


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
                characterAnim.UpdateSpriteToWalk(cachedDirection);
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
        float dTimeRemaining = dashTime;

        dashDirection = direction;
        if (characterAnim.UpdateSpriteToDash(direction)) //UpdateSpriteToDash returns true if it is the default dash
        {
            dashDirection = Vector3.right;
        }
        else
        {
            characterAnim.UpdateSpriteToDash(dashDirection);
        }

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

        characterAnim.UpdateSpriteToWalk(dashDirection);
        cachedDirection = characterAnim.CachedDirToVector();

        DashUI.UpdateDashUI(remainingCharges);
    }

    public IEnumerator RechargeDash()
    {
        canRecharge = false;
        yield return new WaitForSeconds(dashChargeCooldown);
        remainingCharges = dashCharges;
        DashUI.UpdateDashUI(remainingCharges);
        canRecharge = true;
    }

    //  for connor's dash attack
    public float getDashTime()
    {
        return dashTime;
    }
    #endregion

    public void slide(Vector3 dir, float force, float time) {
        if(slider != null)
            return;
        slider = StartCoroutine(slideWaiter(dir, force, time));
    }
    IEnumerator slideWaiter(Vector3 dir, float force, float time) {
        cantDash = true;
        float dTimeRemaining = time;

        while(dTimeRemaining > 0) {
            dTimeRemaining -= Time.deltaTime;
            player.Move(force * Time.deltaTime * dir.normalized);
            yield return null;
        }

        cantDash = false;
        slider = null;
    }
}