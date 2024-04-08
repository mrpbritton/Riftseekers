using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveOverCanvas : MonoBehaviour {
    [SerializeField] List<string> waveOverTexts = new List<string>();
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Transform startPos, endPos;


    private void Update() {
        if(Input.GetKeyDown(KeyCode.U))
            runAnim();
    }

    void runAnim() {
        text.transform.position = startPos.position;
        text.transform.DOMove(endPos.position, .5f);
    }
}
