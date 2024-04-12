using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveCounter : Singleton<WaveCounter>
{
    [SerializeField] private TMP_Text text;
    private Color waveColor = Color.yellow;

    [SerializeField] TextMeshProUGUI moneyText;

    public void OnEnable()
    {
        WaveSpawner.WaveComplete += UpdateCounter;
    }

    private void OnDisable() {
        WaveSpawner.WaveComplete -= UpdateCounter;
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
}
