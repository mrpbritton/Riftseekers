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
    private static Vector3 direction;
    [SerializeField, Tooltip("How fast the player falls")]
    private float fallSpeed;

    Coroutine slider = null;

    private CharacterController player;
    private Vector3 cachedDirection;
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
    }
    private void OnDisable()
    {
        pInput.Disable();
        bonuspicker.EnablePlayerMovement += EnablePlayerMovement;
        bonuspicker.DisablePlayerMovement += DisablePlayerMovement;
    }
    #endregion

    public static Vector3 GetDirection()
    {
        return direction;
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
                SpriteManager.I.UpdateSpriteToWalk(cachedDirection);
            }

            player.Move(PlayerStats.MovementSpeed * Time.deltaTime * direction);

            //if the player isn't grounded, move them towards the ground.
            if (player.isGrounded == false)
                if (player.isGrounded == false)
                {
                    player.Move(fallSpeed * Time.deltaTime * Vector3.down);
                }
        }
    }

    public void slide(Vector3 dir, float force, float time) {
        if(slider != null)
            return;
        slider = StartCoroutine(slideWaiter(dir, force, time));
    }
    IEnumerator slideWaiter(Vector3 dir, float force, float time) {
        //cantDash = true;
        float dTimeRemaining = time;

        while(dTimeRemaining > 0) {
            dTimeRemaining -= Time.deltaTime;
            player.Move(force * Time.deltaTime * dir.normalized);
            yield return null;
        }

        //cantDash = false;
        slider = null;
    }
}