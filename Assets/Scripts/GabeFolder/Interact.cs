using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameActionSequence))]
[RequireComponent(typeof(SphereCollider))]
public class Interact : MonoBehaviour
{
    private PInput pInput;
    protected GameObject player;

    [SerializeField, Tooltip("Range the player can be from the interactable to interact with it")]
    protected float interactRange;
    protected SphereCollider interactCollider;
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
        }

        //The reason we are doing an AddComponent instead of a GetComponent
        //  is to make sure that if the item is a sphere by default, it doesn't
        //  override the current collider

        interactCollider = gameObject.GetComponent<SphereCollider>();
        interactCollider.name = "interactCollider";
        interactCollider.isTrigger = true;
        interactCollider.radius = interactRange;

        interactSequence = GetComponent<GameActionSequence>();
    }

    protected virtual void Interacted() { /*override me*/ }

    private void OnTriggerEnter(Collider other)
    {
        canInteract = true;
    }

    private void OnTriggerExit(Collider other)
    {
        canInteract = false;
    }
}
