using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputAction Movement;
    public InputAction DashInput;

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
    private void Start()
    {
        Movement.Enable();
        DashInput.Enable();
        if (dashSpeed == 0) //if the dashSpeed isn't defined
        {
            dashSpeed = speed * 2;
        }
        DashInput.performed += DashPress;
    }
    private void OnDisable()
    {
        Movement.Disable();
        DashInput.Disable();
        DashInput.performed -= DashPress;
    }

    private void DashPress(InputAction.CallbackContext c)
    {
        if (cantDash) return;
        if (direction == Vector3.zero) //if no movement, dash right
            direction.x = 1;
        StartCoroutine(Dash(direction));
    }

    private void Update()
    {
        direction.x = Movement.ReadValue<Vector2>().x;
        direction.z = Movement.ReadValue<Vector2>().y;
        direction.y = 0;

        transform.position += direction * Time.deltaTime * speed;
    }

    IEnumerator Dash(Vector3 localDir)
    {
        cantDash = true;
        Movement.Disable();
        float dTimeRemaining = dashTime;

        /* The loop below ends up being a pseudo-update function. This is able to 
         * happen because of the yield return null; at the end of this while loop.
         * Every iteration makes the dTimeRemaining decrease until it is zero (or 
         * until the dash is completed), while the position of the player continues 
         * to move towards the direction */

        while (dTimeRemaining > 0)
        {
            dTimeRemaining -= Time.deltaTime;
            transform.position += localDir * Time.deltaTime * dashSpeed;
            yield return null;
        }
        Movement.Enable();
        yield return new WaitForSeconds(dashCooldown);
        cantDash = false;
    }
}
