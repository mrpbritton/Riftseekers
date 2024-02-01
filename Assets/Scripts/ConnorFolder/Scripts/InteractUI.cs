using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class InteractUI : MonoBehaviour {
    [SerializeField] Transform background;
    [SerializeField] string helpText;
    [SerializeField] float yOffset;
    float shownScale;

    KdTree<Transform> interactables = new KdTree<Transform>();

    Transform closestInteractable;
    Transform playerTrans;
    FacesCamera fc;

    Coroutine checker = null;

    bool initted = false;
    bool shown = false;
    bool able = false;

    private void Start() {
        shownScale = background.transform.localScale.x;
        background.transform.localScale = new Vector3(background.transform.localScale.x, 0f, 0f);
        able = true;
    }

    private void OnDisable() {
        if(checker != null)
            StopCoroutine(checker);
    }

    public void addInteractable(Transform ting) {
        if(!able) {
            StartCoroutine(waiter(ting));
            return;
        }
        if(!initted || playerTrans == null) {
            DOTween.Init();
            playerTrans = FindObjectOfType<PlayerMovement>().transform;
            fc = background.GetComponent<FacesCamera>();
            background.GetComponentInChildren<TextMeshProUGUI>().text = helpText;
            initted = true;
        }
        interactables.Add(ting);
        closestInteractable = interactables.FindClosest(playerTrans.position);
        if(checker == null)
            checker = StartCoroutine(interactableChecker());
    }

    IEnumerator waiter(Transform ting) {
        while(!able)
            yield return new WaitForEndOfFrame();
        addInteractable(ting);
    }


    void show(Transform interactable) {
        shown = true;
        fc.enabled = true;
        background.DOComplete();
        background.position = interactable.position;
        background.localScale = Vector3.zero;
        background.DOLocalMoveY(background.localPosition.y + yOffset, .15f);
        background.DOScale(shownScale, .15f);
    }

    void hide() {
        shown = false;
        fc.enabled = false;
        background.DOComplete();
        background.DOLocalMoveY(background.localPosition.y - yOffset, .25f);
        background.DOScale(0f, .25f);
    }

    public void completeInteraction(Transform interactable) {
        interactables.RemoveAll(x => x == interactable);
        closestInteractable = interactables.Count > 0 ? interactables.FindClosest(playerTrans.position) : null;
        if(interactables.Count <= 0 && checker != null) {
            StopCoroutine(checker);
            checker = null;
        }
        hide();
    }

    IEnumerator interactableChecker() {
        if(interactables.Count > 0 && playerTrans != null) {
            closestInteractable = interactables.FindClosest(playerTrans.position);
            var t = Vector2.Distance(playerTrans.position, closestInteractable.position) / 20f;
            //  checks if within interact range
            if(!shown && closestInteractable != null && closestInteractable.GetComponent<Interact>().inRange())
                show(closestInteractable.transform);
            else if(shown && !closestInteractable.GetComponent<Interact>().inRange())
                hide();

            yield return new WaitForSeconds(t);
            checker = StartCoroutine(interactableChecker());
        }
        else
            checker = null;
    }
}
