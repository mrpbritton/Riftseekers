using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private PInput pInput;
    private GameObject player;

    [SerializeField, Tooltip("How far away the player can be from the item to pick it up")]
    private float pickUpRange;
    

    private void Start()
    {
        pInput.Enable();

        player = GameObject.FindGameObjectWithTag("Player");
        pInput.Player.Ability2.performed += ctxt => PickUp();
    }

    private void PickUp()
    {

    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.position - player.transform.position, Color.red, pickUpRange);
    }
}
