using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShopVendor : MonoBehaviour {

    PInput input;

    [SerializeField] Canvas shopWindow;
    [SerializeField] float maxDistFromPlayer;

    bool interacting = false;


    private void Start() {
        hide();
        input = new PInput();
        input.Enable();
        input.Player.Interact.performed += ctx => toggleInteract();
    }

    private void OnDisable() {
        input.Disable();
    }


    void toggleInteract() {
        if(interacting)
            tryDeinteract();
        else
            tryInteract();
    }

    public void tryInteract() {
        //  checks the dist from player
        var pPos = FindObjectOfType<CharacterFrame>();
        if(Vector3.Distance(pPos.transform.position, transform.position) > maxDistFromPlayer)
            return;

        interacting = true;
        show();
        interact();
    }
    public void tryDeinteract() {
        interacting = false;
        hide();
        deinteract();
    }

    void show() {
        shopWindow.gameObject.SetActive(true);
    }
    void hide() {
        shopWindow.gameObject.SetActive(false);
    }

    protected abstract void interact();
    protected abstract void deinteract();
}
