using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuInstance : MonoBehaviour {

    PInput input;

    [SerializeField] Canvas shopWindow;
    [SerializeField] float maxDistFromPlayer;

    PlayerMovement pm;

    public bool interacting { get; private set; } = false;


    private void Start() {
        pm = FindObjectOfType<PlayerMovement>();
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
        //  checks if other menus are opened
        foreach(var i in FindObjectsOfType<MenuInstance>()) {
            if(i != gameObject && i.interacting)
                return;
        }
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
        if(shopWindow != null)
            shopWindow.gameObject.SetActive(true);
        pm.enabled = false;
    }
    void hide() {
        if(shopWindow != null)
            shopWindow.gameObject.SetActive(false);
        pm.enabled = true;
    }

    protected abstract void interact();
    protected abstract void deinteract();
}
