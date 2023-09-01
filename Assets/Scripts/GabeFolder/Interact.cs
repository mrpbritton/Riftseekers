using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    private PInput pInput;
    protected GameObject player;

    [SerializeField, Tooltip("How far away the player can be from the interactable to interact with it")]
    protected float interactRange;
    private Ray interactRay;
    protected bool canInteract;
    [SerializeField, Tooltip("Sequence exectuted when interacted with.")]
    protected GameActionSequence interactSequence;

    protected void Start()
    {
        pInput = new();
        pInput.Enable();
        pInput.Player.Ability2.performed += ctxt => Interacted();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void Interacted() { /*override me*/ }

    protected void Update()
    {
        interactRay.origin = transform.position;
        interactRay.direction = (player.transform.position - transform.position).normalized;
        Debug.DrawRay(interactRay.origin, interactRay.direction * interactRange, Color.green);

        canInteract = Physics.Raycast(interactRay.origin, interactRay.direction, interactRange);
    }
}
