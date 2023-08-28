using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public PInput pInput;

    [Header("Regular Movement")]
    [SerializeField, Tooltip("How fast the player moves")]
    private float speed;
    private Vector3 direction;

    [Header("Dash")]
    [SerializeField, Tooltip("Cooldown of the dash ability in seconds")]
    private float dashCooldown;
    [SerializeField, Tooltip("How long the dash takes in seconds")]
    private float dashTime;
    [SerializeField, Tooltip("Speed of the dash")]
    private float dashSpeed;
    private bool cantDash;

    private CharacterController player;

    private void Start()
    {
        pInput = new PInput();
        pInput.Enable();   
        player = GetComponent<CharacterController>();
        pInput.Player.Dash.started += DashPress;
    }
    private void OnDisable()
    {
        pInput.Disable();
        pInput.Player.Dash.started -= DashPress;
    }

    private void DashPress(InputAction.CallbackContext c)
    {
        if (cantDash) return;
        if (direction == Vector3.zero) //if no movement, dash right
            direction.x = 1;
        StartCoroutine(Dash(direction));
    }
    //cbt otherwise known as cock and ball torture
    private void Update()
    {
        direction.x = pInput.Player.Movement.ReadValue<Vector3>().x;
        direction.z = pInput.Player.Movement.ReadValue<Vector3>().z;
        direction.y = 0;

        switch(direction.x, direction.z)
        {
            case (1, -1):
                direction.x -= 1;
                break;
            case (1, 0):
                direction.x -= 1;
                break;
            case (1, 1):
                direction.x -= 1;
                break;
            case (0, 1):
                direction.z += 1;
                break;
            case (0, -1):
                direction.z -= 1;
                break;
            case (-1, -1):
                direction.x += 1;
                break;
            case (-1, 0):
                direction.x += 1;
                break;
            case (-1, 1):
                direction.z += 1;
                break;
            default:
                break;
        }

        player.Move(speed * Time.deltaTime * direction.normalized);
    }

    IEnumerator Dash(Vector3 localDir)
    {
        cantDash = true;
        float dTimeRemaining = dashTime;

        /* The loop below ends up being a pseudo-update function. This is able to 
         * happen because of the yield return null; at the end of this while loop.
         * Every iteration makes the dTimeRemaining decrease until it is zero (or 
         * until the dash is completed), while the position of the player continues 
         * to move towards the direction */

        while (dTimeRemaining > 0)
        {
            dTimeRemaining -= Time.deltaTime;
            player.Move(localDir * dashSpeed);
            yield return null;
        }
        yield return new WaitForSeconds(dashCooldown);
        cantDash = false;
    }
}