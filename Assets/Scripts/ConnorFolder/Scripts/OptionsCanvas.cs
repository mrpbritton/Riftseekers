using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AK;

public class OptionsCanvas : Singleton<OptionsCanvas> {
    [SerializeField] Slider masterVolSlider;
    [SerializeField] GameObject background;
    [SerializeField] TextMeshProUGUI screenModeText;

    FullScreenMode curScreenMode;

    public static Action settingsChanged = delegate { };

    private void Start() {
        setup();
        background.SetActive(false);
    }

    void setup() {
        //  sound
        var master = SaveData.getUniversalFloat("masterVolume", 1f);
        masterVolSlider.value = master;

        //  screen mode
        curScreenMode = (FullScreenMode)SaveData.getUniversalInt("screenMode", (int)FullScreenMode.ExclusiveFullScreen);
        Screen.fullScreenMode = curScreenMode;
        updateScreenModeText();
        settingsChanged();
        StartCoroutine(delaySettingsChanged());
    }

    public void show() {
        setup();
        background.SetActive(true);
        masterVolSlider.Select();
    }
    public void hide() {
        background.SetActive(false);
    }

    public bool isOpen() {
        return background.activeInHierarchy;
    }

    public void screenModeToggle(bool right) {
        int cur = (int)curScreenMode;
        cur = right ? cur + 1 : cur - 1;
        if(cur < 0)
            cur = 3;
        else if(cur > 3)
            cur = 0;
        curScreenMode = (FullScreenMode)cur;
        updateScreenModeText();
    }
    void updateScreenModeText() {
        screenModeText.text = curScreenMode == FullScreenMode.ExclusiveFullScreen ? "Fullscreen" : curScreenMode == FullScreenMode.FullScreenWindow ? "Windowed Fullscreen" :
            curScreenMode == FullScreenMode.MaximizedWindow ? "Borderless Window" : "Windowed";
    }
    
    public void apply() {
        //  sound
        SaveData.setUniversalFloat("masterVolume", masterVolSlider.value);

        //  screen mode
        SaveData.setUniversalInt("screenMode", (int)curScreenMode);
        Screen.fullScreenMode = curScreenMode;

        settingsChanged();
    }
    public void revert() {
        masterVolSlider.value = 1f;
        curScreenMode = FullScreenMode.ExclusiveFullScreen;
        updateScreenModeText();
        apply();
    }
    public float getVolume() {
        return masterVolSlider.value;
    }

    IEnumerator delaySettingsChanged() {
        yield return new WaitForSeconds(.1f);
        settingsChanged();
    }
}
