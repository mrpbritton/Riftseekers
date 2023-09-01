using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    private PInput pInput;
    protected GameObject player;
    protected CharacterFrame frame;

    [SerializeField, Tooltip("How far away the player can be from the interactable to interact with it")]
    protected float interactRange;
    private Ray interactRay;
    protected bool canInteract;
    [SerializeField, Tooltip("Sequence exectuted when interacted with.")]
    protected GameActionSequence interactSequence;

    protected virtual void Start()
    {
        pInput = new();
        pInput.Enable();
        pInput.Player.Interact.performed += ctxt => Interacted();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            frame = player.GetComponent<CharacterFrame>();
        }
    }

    protected virtual void Interacted() { /*override me*/ }

    protected virtual void Update()
    {
        interactRay.origin = transform.position;
        interactRay.direction = (player.transform.position - transform.position).normalized;
        Debug.DrawRay(interactRay.origin, interactRay.direction * interactRange, Color.green);

        canInteract = Physics.Raycast(interactRay.origin, interactRay.direction, interactRange);
    }
}
