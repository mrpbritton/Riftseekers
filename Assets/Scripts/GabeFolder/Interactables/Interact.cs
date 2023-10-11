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
    }

    protected void OnDestroy()
    {
        pInput.Player.Interact.performed -= ctxt => Interacted();
        pInput.Disable();
    }

    protected virtual void Interacted() 
    {
        var pPos = new Vector2(player.transform.position.x, player.transform.position.z);
        var mePos = new Vector2(transform.position.x, transform.position.z);
        if (Vector2.Distance(pPos, mePos) > interactRange) return; //if cant interact, return
        interactSequence.Play();
    }
}
