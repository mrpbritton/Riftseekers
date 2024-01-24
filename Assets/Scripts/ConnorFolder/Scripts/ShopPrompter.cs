using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopPrompter : MonoBehaviour {
    [SerializeField] GameObject canvas;
    PInput pInput;

    bool shown = false;

    InteractUI helperUI;
    Interact interact;

    private void Start() {
        pInput = new PInput();
        pInput.Enable();
        pInput.Player.Interact.performed += ctx => toggleShownState();

        interact = GetComponent<Interact>();

        helperUI = FindObjectOfType<InteractUI>();
        helperUI.addInteractable(transform);

        hide();
    }

    private void OnDisable() {
        pInput.Disable();
    }

    void toggleShownState() {
        if(!interact.inRange())
            return;
        if(shown)
            hide();
        else
            show();
        Debug.Log(shown);
    }

    void show() {
        shown = true;
        helperUI.completeInteraction(transform);
        canvas.SetActive(true);
    }
    void hide() {
        shown = false;
        canvas.SetActive(false);
    }

    public void buy() {
        Debug.Log("bought");
    }
    public void sell() {
        Debug.Log("sold");
    }
}
