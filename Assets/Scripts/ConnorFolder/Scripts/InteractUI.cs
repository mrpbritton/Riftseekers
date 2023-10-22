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

    private void Awake() {
        shownScale = background.transform.localScale.x;
        background.transform.localScale = new Vector3(background.transform.localScale.x, 0f, 0f);
    }

    public void addInteractable(Transform ting) {
        if(!initted) {
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
        if(interactables.Count <= 0) {
            StopCoroutine(checker);
            checker = null;
        }
        hide();
    }

    IEnumerator interactableChecker() {
        if(interactables.Count > 0) {
            closestInteractable = interactables.FindClosest(playerTrans.position);
            var d = Vector3.Distance(closestInteractable.transform.position, playerTrans.position);
            var ir = closestInteractable.GetComponent<Interact>().getInteractRange();
            var t = Mathf.Abs(ir - d) / 10f + .01f;
            //  checks if within interact range
            if(!shown && d < ir)
                show(closestInteractable.transform);
            else if(shown && d > ir)
                hide();

            yield return new WaitForSeconds(t);
            checker = StartCoroutine(interactableChecker());
        }
        else
            checker = null;
    }
}
