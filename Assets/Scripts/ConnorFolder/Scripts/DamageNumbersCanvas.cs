using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class DamageNumbersCanvas : MonoBehaviour {
    [SerializeField] GameObject textPrefab;
    [SerializeField] float offset;

    [SerializeField] Color lowColor, medColor, highColor;

    private void Start() {
        MonsterHealth.runOnHurt += showText;
    }

    private void OnDisable() {
        MonsterHealth.runOnHurt -= showText;
    }

    void showText(int dmg, Transform trans) {
        var obj = Instantiate(textPrefab, transform);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = dmg.ToString();
        StartCoroutine(textAnim(dmg, obj, trans, obj.GetComponentInChildren<TextMeshProUGUI>()));
    }

    IEnumerator textAnim(int dmg, GameObject obj, Transform trans, TextMeshProUGUI text) {
        obj.transform.position = trans.position;
        obj.transform.localScale = Vector3.zero;
        obj.transform.DOMoveY(trans.position.y + offset, .15f);
        obj.transform.DOScale(1f, .15f);
        text.color = getRelevantColor(dmg);

        yield return new WaitForSeconds(1f);
        obj.transform.DOScale(1.25f, .25f);
        obj.transform.DOMoveY(obj.transform.position.y + offset / 2f, .25f);
        text.DOColor(Color.clear, .25f);
        Destroy(obj, .26f);
    }

    Color getRelevantColor(int dmg) {
        if(dmg < 2)
            return lowColor;
        if(dmg < 3)
            return medColor;
        return highColor;
    }
}
