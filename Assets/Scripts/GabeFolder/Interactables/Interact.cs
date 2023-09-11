using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(GameActionSequence))]
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

    protected virtual void Start() { }
    protected void Awake()
    {
        pInput = new();
        pInput.Enable();
        pInput.Player.Interact.performed += ctxt => Interacted();
        
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        interactCollider = gameObject.GetComponent<SphereCollider>();
        interactCollider.radius = interactRange;
    }

    protected void OnDestroy()
    {
        pInput.Player.Interact.performed -= ctxt => Interacted();
        pInput.Disable();
    }

    protected virtual void Interacted() 
    {
        if (!canInteract) return; //if cant interact, return
        interactSequence.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        canInteract = true;
    }

    private void OnTriggerExit(Collider other)
    {
        canInteract = false;
    }
}
