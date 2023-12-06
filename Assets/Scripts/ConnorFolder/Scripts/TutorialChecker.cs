using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TutorialChecker : MonoBehaviour {

    PInput controls;

    public enum teachTypes {
        None, Move, Dash, BasicAtt, SecondAtt, RocketAtt, Interact, End
    }

    [System.Serializable]
    public class tutText {
        public TextMeshProUGUI text;
        public teachTypes teachType;
        public int room;
    }

    [SerializeField] List<tutText> texts = new List<tutText>();

    teachTypes curTeching;
    Coroutine rotator = null;

    List<Vector2> roomPoses = new List<Vector2>();

    Transform playerTrans;

    private void Awake() {
        playerTrans = FindObjectOfType<PlayerMovement>().transform;
        controls = new PInput();
        controls.Enable();

        //  First Room (movment)
        controls.Player.Movement.performed += ctx => { tryRotate(teachTypes.Move); };
        controls.Player.Dash.performed += ctx => { tryRotate(teachTypes.Dash); };

        //  Second Room (combat intro)
        controls.Player.BasicAttack.performed += ctx => { tryRotate(teachTypes.BasicAtt); };
        controls.Player.SecondAttack.performed += ctx => { tryRotate(teachTypes.SecondAtt); };
        controls.Player.Ability1.performed += ctx => { tryRotate(teachTypes.RocketAtt); };

        //  Third Room (pickups / items / new abilities)
        controls.Player.Interact.performed += ctx => { tryRotate(teachTypes.Interact); };

        //  Fourth Room (putting everything together / advanced combat)


        //  combines all texts into the tutText
        foreach(var i in texts) {
            i.text.enabled = false;
            if(i.room == roomPoses.Count)
                roomPoses.Add(i.text.transform.position);
        }
        curTeching = teachTypes.Move;
        showCurText();
    }

    private void OnDisable() {
        controls.Disable();
    }

    void tryRotate(teachTypes type) {
        if(rotator != null)
            return;
        rotator = StartCoroutine(rotate(type));
    }

    IEnumerator rotate(teachTypes type) {
        if(type < curTeching || texts.Count == 0)
            yield break;
        hideCurText();
        yield return new WaitForSeconds(.16f);
        curTeching++;
        if(curTeching != teachTypes.End) {
            do
                yield return new WaitForEndOfFrame();
            while(getPlayerRoom() != texts[(int)curTeching - 1].room);
            showCurText();
        }
        rotator = null;
    }

    void showCurText() {
        texts[(int)curTeching - 1].text.enabled = true;
        texts[(int)curTeching - 1].text.DOComplete();
        texts[(int)curTeching - 1].text.DOColor(Color.white, .15f);
        texts[(int)curTeching - 1].text.transform.DOScale(1.1f, .15f);
    }

    void hideCurText() {
        texts[(int)curTeching - 1].text.DOComplete();
        texts[(int)curTeching - 1].text.DOColor(Color.clear, .15f);
        texts[(int)curTeching - 1].text.transform.DOScale(0f, .15f);
    }

    public int getPlayerRoom() {
        float closest = -1f;
        int curRoom = -1;
        for(int i = 0; i < roomPoses.Count; i++) {
            var dist = Vector2.Distance(roomPoses[i], playerTrans.position);
            if(closest == -1 || dist < closest) {
                closest = dist;
                curRoom = i;
            }
        }
        return curRoom;
    }
}
