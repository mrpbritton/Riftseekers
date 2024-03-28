using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveCounter : Singleton<WaveCounter>
{
    [SerializeField] private TMP_Text text;
    private Color waveColor = Color.yellow;
    public int WaveNum { get; private set; }

    public void OnEnable()
    {
        WaveSpawner.WaveComplete += UpdateCounter;
        UpdateCounter();
    }

    public void UpdateCounter()
    {
        WaveNum++;
        waveColor.g -= 0.01f;
        UpdateColor();
    }

    public void ResetCounter()
    {
        WaveNum = 0;
        waveColor = Color.yellow;
        UpdateColor();
    }

    private void UpdateColor()
    {
        text.text = $"<color=#{ColorUtility.ToHtmlStringRGB(waveColor)}>" + "wave: "
                  + $"<color=white>" + WaveNum;
    }
}
