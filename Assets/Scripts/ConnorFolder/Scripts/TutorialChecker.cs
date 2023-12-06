using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Linq;

public class TutorialChecker : MonoBehaviour {

    PInput controls;

    public enum teachTypes {
        None, Move, Dash, BasicAtt, SecondAtt, RocketAtt, ClearFirstEnemies, Interact, Pickup, Equip, Shotgun, ClearSecondEnemies, Teleport, End
    }

    [System.Serializable]
    public class tutText {
        public TextMeshProUGUI text;
        public teachTypes teachType;
        public int room;
    }

    [SerializeField] List<tutText> texts = new List<tutText>();
    [SerializeField] List<GameObject> blockers = new List<GameObject>();

    teachTypes curTeaching;
    Coroutine rotator = null;

    List<Vector2> roomPoses = new List<Vector2>();

    Transform playerTrans;

    int startingEnemyCount, curEnemyCount;

    private void Awake() {
        SaveData.wipe();    //  THIS LINE MIGHT BREAK THINGS LATER
        playerTrans = FindObjectOfType<PlayerMovement>().transform;
        controls = new PInput();
        controls.Enable();

        //  First Room (movment)
        controls.Player.Movement.performed += ctx => { tryRotate(teachTypes.Move); };
        controls.Player.Dash.performed += ctx => { tryRotate(teachTypes.Dash); };

        //  Second Room (combat intro)
        controls.Player.BasicAttack.performed += ctx => { tryRotate(teachTypes.BasicAtt); };
        controls.Player.SecondAttack.performed += ctx => { tryRotate(curTeaching == teachTypes.SecondAtt ? teachTypes.SecondAtt : teachTypes.Shotgun); };
        controls.Player.Ability1.performed += ctx => { tryRotate(teachTypes.RocketAtt); };

        //  Third Room (pickups / items / new abilities)
        controls.Player.Interact.performed += ctx => { tryRotate(curTeaching == teachTypes.Interact ? teachTypes.Interact : teachTypes.Pickup); };

        //  Fourth Room (putting everything together / advanced combat)


        //  combines all texts into the tutText
        foreach(var i in texts) {
            i.text.enabled = false;
            i.text.color = Color.clear;
            i.text.transform.localScale = Vector3.one;
            if(i.room == roomPoses.Count)
                roomPoses.Add(i.text.transform.position);
        }
        curTeaching = teachTypes.Move;
        showCurText();

        StartCoroutine(enemyCounter(FindObjectsOfType<EnemyHealth>().ToList()));
        StartCoroutine(waitForShotgunEquipped());
    }

    private void OnDisable() {
        controls.Disable();
    }

    public void tryRotate(teachTypes type) {
        if(rotator != null || texts.Count == 0 || type != curTeaching) 
            return;
        rotator = StartCoroutine(rotate());
    }

    IEnumerator rotate() {
        hideCurText();
        yield return new WaitForSeconds(.16f);
        curTeaching++;
        if((int)curTeaching - 1 < texts.Count) {
            //  unblocks
            if(texts[(int)curTeaching - 1].room > 0)
                blockers[texts[(int)curTeaching - 1].room - 1].SetActive(false);

            //  waits for player to move into the correct room
            do
                yield return new WaitForEndOfFrame();
            while(getPlayerRoom() != texts[(int)curTeaching - 1].room);

            //  shows the thing
            showCurText();
        }
        if(curTeaching == teachTypes.End || curTeaching == teachTypes.Teleport) {
            foreach(var i in blockers)
                i.SetActive(false);
        }
        rotator = null;
    }

    IEnumerator enemyCounter(List<EnemyHealth> enemies) {
        startingEnemyCount = enemies.Count;
        curEnemyCount = startingEnemyCount;
        while(enemies.Count > 0) {
            for(int i = 0; i < enemies.Count; i++) {
                //  checks if this enemy no longer exists
                if(enemies[i] == null || enemies[i].gameObject == null) {
                    curEnemyCount--;
                    //  runs the thing
                    if(startingEnemyCount - curEnemyCount == 2) {
                        tryRotate(teachTypes.ClearFirstEnemies);
                    }
                    enemies.RemoveAt(i);
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }
        tryRotate(teachTypes.ClearSecondEnemies);
    }

    IEnumerator waitForShotgunEquipped() {
        var it = FindObjectOfType<ItemLibrary>();
        while(true) {
            yield return new WaitForEndOfFrame();
            bool hasShotty = Inventory.getActiveItem(0, it) != null ||
                Inventory.getActiveItem(1, it) != null ||
                Inventory.getActiveItem(2, it) != null;
            if(hasShotty) {
                tryRotate(teachTypes.Equip);
                break;
            }
        }
    }

    void showCurText() {
        texts[(int)curTeaching - 1].text.enabled = true;
        texts[(int)curTeaching - 1].text.DOComplete();
        texts[(int)curTeaching - 1].text.DOColor(Color.white, .15f);
        texts[(int)curTeaching - 1].text.transform.DOScale(1.1f, .15f);
    }

    void hideCurText() {
        texts[(int)curTeaching - 1].text.DOComplete();
        texts[(int)curTeaching - 1].text.DOColor(Color.clear, .15f);
        texts[(int)curTeaching - 1].text.transform.DOScale(0f, .15f);
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
