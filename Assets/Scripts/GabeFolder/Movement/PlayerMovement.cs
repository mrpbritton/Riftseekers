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

    private void OnEnable()
    {
        pInput = new PInput();
        pInput.Enable();   
        player = GetComponent<CharacterController>();
        frame = GetComponent<CharacterFrame>();
        speed = frame.movementSpeed;
        dashSpeed = frame.dashSpeed;
        pInput.Player.Dash.started += DashPress;

        CharacterFrame.UpdateStats += UpdateStats;
        canRecharge = true;
        remainingCharges = frame.dashCharges;
    }
    private void OnDisable()
    {
        pInput.Disable();
        pInput.Player.Dash.started -= DashPress;
    }

    private void UpdateStats()
    {
        speed = frame.movementSpeed;
        dashSpeed = frame.dashSpeed;
        dashDistance = frame.dashDistance;
        dashCharges = frame.dashCharges;
        dashCooldown *= frame.cooldownMod;
        dashChargeCooldown *= frame.cooldownMod;
    }

    private void DashPress(InputAction.CallbackContext c)
    {
        if (cantDash || remainingCharges == 0) return;
        StartCoroutine(Dash());
    }
    //cbt otherwise known as cock and ball torture
    private void Update()
    {
        direction.x = pInput.Player.Movement.ReadValue<Vector3>().x;
        direction.z = pInput.Player.Movement.ReadValue<Vector3>().z;

        player.Move(speed * Time.deltaTime * direction);

        //if the player isn't grounded, move them towards the ground.
        if(player.isGrounded == false) 
        {
            player.Move(fallSpeed * Time.deltaTime * Vector3.down);
        }

        if(remainingCharges < dashCharges && canRecharge)
        {
            StartCoroutine(RechargeDash());
        }
    }

    public IEnumerator Dash()
    {
        cantDash = true;
        float dTimeRemaining = dashTime;

        if(direction == Vector3.zero) //if no movement, dash right
            direction.x = 1;

        /* The loop below ends up being a pseudo-update function. This is able to 
         * happen because of the yield return null; at the end of this while loop.
         * Every iteration makes the dTimeRemaining decrease until it is zero (or 
         * until the dash is completed), while the position of the player continues 
         * to move towards the direction */

        while (dTimeRemaining > 0)
        {
            dTimeRemaining -= Time.deltaTime;
            player.Move(direction * dashSpeed * Time.deltaTime * dashDistance);
            yield return null;
        }

        yield return new WaitForSeconds(dashCooldown);
        remainingCharges--; //since dash was performed, subtract a dash
        cantDash = false;
    }

    public IEnumerator RechargeDash()
    {
        canRecharge = false;
        Debug.Log("Recharging...");
        yield return new WaitForSeconds(dashChargeCooldown);
        remainingCharges++;
        canRecharge = true;
    }

    //  for connor's dash attack
    public float getDashTime() 
    {
        return dashTime;
    }
}