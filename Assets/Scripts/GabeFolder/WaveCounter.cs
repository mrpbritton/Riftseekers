using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class WaveCounter : Singleton<WaveCounter>
{
    [SerializeField] private TMP_Text text;
    private Color waveColor = Color.yellow;

    [SerializeField] TextMeshProUGUI moneyText;
    float curMoneyShown = 0f;

    Transform tweeningObj;

    private void Start() {
        StartCoroutine(thing());
    }

    IEnumerator thing() {
        yield return new WaitForEndOfFrame();
        tweeningObj = new GameObject().transform;
        tweeningObj.parent = transform;
        moneyText.text = "money: " + Inventory.getMoney().ToString("0.0");
    }

    public void OnEnable()
    {
        WaveSpawner.WaveComplete += UpdateCounter;
        Inventory.moneyChanged += updateMoneyText;
    }

    private void OnDisable() {
        WaveSpawner.WaveComplete -= UpdateCounter;
        Inventory.moneyChanged -= updateMoneyText;
    }

    public void UpdateCounter()
    {
        waveColor.g -= 0.01f;
        UpdateColor();
    }

    public void ResetCounter()
    {
        waveColor = Color.yellow;
        UpdateColor();
    }

    private void UpdateColor()
    {
        text.text = $"<color=#{ColorUtility.ToHtmlStringRGB(waveColor)}>" + "wave: "
                  + $"<color=white>" + WaveSpawner.I.waveIndex;
    }

    void updateMoneyText() {
        tweeningObj.DOKill();
        tweeningObj.localPosition = Vector3.right * curMoneyShown;
        tweeningObj.DOMoveX(Inventory.getMoney(), .25f).OnUpdate(() => {
            moneyText.text = "money: " + tweeningObj.localPosition.x.ToString("0.0");
            curMoneyShown = tweeningObj.localPosition.x;
        });
    }
}
