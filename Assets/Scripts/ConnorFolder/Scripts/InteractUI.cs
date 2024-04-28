using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class InteractUI : Singleton<InteractUI> {
    [SerializeField] Transform background;
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] string helpText;
    [SerializeField] float yOffset;
    float shownScale;

    KdTree<Transform> interactables = new KdTree<Transform>();
    List<InteractInfo> infos = new List<InteractInfo>();

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

    public void addInteractable(Transform ting, InteractInfo info) {
        if(!able) {
            StartCoroutine(waiter(ting, info));
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
        infos.Add(info);
        closestInteractable = interactables.FindClosest(playerTrans.position);
        if(checker == null)
            checker = StartCoroutine(interactableChecker(info));
    }

    IEnumerator waiter(Transform ting, InteractInfo info) {
        while(!able)
            yield return new WaitForEndOfFrame();
        addInteractable(ting, info);
    }


    void show(Transform interactable, InteractInfo info) {
        shown = true;
        fc.enabled = true;
        background.DOComplete();
        background.position = interactable.position;
        background.localScale = Vector3.zero;
        background.DOLocalMoveY(background.localPosition.y + yOffset, .15f);
        background.DOScale(shownScale, .15f);

        //  Do stuff with info
        for(int i = 0; i < interactables.Count; i++) {
            if(interactables[i] == interactable) {
                infoText.text = "<color=yellow>" + infos[i].type.ToString() + "<color=white>: " + infos[i].title;
                break;
            }
        }
    }

    void hide() {
        shown = false;
        fc.enabled = false;
        background.DOComplete();
        background.DOLocalMoveY(background.localPosition.y - yOffset, .25f);
        background.DOScale(0f, .25f);
    }

    public void completeInteraction(Transform interactable, bool dontSetMeTrue = false) {
        if(dontSetMeTrue) return;
        for(int i = 0; i < interactables.Count; i++) {
            if(interactables[i] == interactable) {
                infos.RemoveAt(i);
                interactables.RemoveAt(i);
            }
        }
        closestInteractable = interactables.Count > 0 ? interactables.FindClosest(playerTrans.position) : null;
        if(interactables.Count <= 0 && checker != null) {
            StopCoroutine(checker);
            checker = null;
        }
        hide();
    }

    IEnumerator interactableChecker(InteractInfo info) {
        if(interactables.Count > 0 && playerTrans != null) {
            closestInteractable = interactables.FindClosest(playerTrans.position);
            //  checks if within interact range
            if(!shown && closestInteractable != null && closestInteractable.GetComponent<Interact>().inRange())
                show(closestInteractable.transform, info);
            else if(shown && !closestInteractable.GetComponent<Interact>().inRange())
                hide();

            yield return new WaitForFixedUpdate();
            if(info != null)
                checker = StartCoroutine(interactableChecker(info));
        }
        else
            checker = null;
    }
}

[System.Serializable]
public class InteractInfo {
    public enum interactType {
        None, Item, Augment, Chest, Shop, Health
    }
    public string title;
    public interactType type;

    public InteractInfo(string title, interactType type) {
        this.title = title;
        this.type = type;
    }
}