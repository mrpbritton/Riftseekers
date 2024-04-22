using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveOverCanvas : Singleton<WaveOverCanvas> {
    [SerializeField] List<string> waveOverTexts = new List<string>();
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Transform startPos, endPos;


    private void Start() {
        WaveSpawner.WaveComplete += runAnim;
    }
    private void OnDisable() {
        WaveSpawner.WaveComplete -= runAnim;
    }

    public void runAnim() {
        StartCoroutine(animWaiter());
    }

    IEnumerator animWaiter() {
        float stepTime = .35f;
        text.gameObject.SetActive(true);
        text.text = waveOverTexts[Random.Range(0, waveOverTexts.Count)];
        text.transform.position = startPos.position;
        text.transform.DOMove(endPos.position, 2f);

        text.transform.localScale = Vector3.zero;
        text.transform.DOScale(1f, stepTime);
        text.color = Color.clear;
        text.DOColor(Color.white, stepTime);
        yield return new WaitForSeconds(2f - stepTime);
        text.DOColor(Color.clear, stepTime);
        text.transform.DOScale(0f, stepTime);
        yield return new WaitForSeconds(stepTime);
        text.gameObject.SetActive(false);
    }
}
