using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using TMPro;

public class TutorialCanvas : MonoBehaviour {
    [SerializeField] TextMeshProUGUI text;
    List<Transform> spawnedTexts = new List<Transform>();
    TextMeshProUGUI curText;
    float spawnHeight;

    private void Start() {
        spawnHeight = FindObjectOfType<NavMeshModifier>().transform.position.y + .1f;
        text.gameObject.SetActive(false);
    }

    public void setPos(Transform pos) {
        var targetPos = new Vector3(pos.position.x, spawnHeight, pos.position.z);
        var obj = Instantiate(text.gameObject, targetPos, text.transform.rotation, transform);
        obj.gameObject.SetActive(true);
        spawnedTexts.Add(obj.transform);
        curText = obj.GetComponent<TextMeshProUGUI>();
    }
    public void showText(string text) {
        curText.text = "";
        for(int i = 0; i < text.Length; i++)
            curText.text += text[i] == '^' ? "\n" : text[i];
    }
    public void hideText(Transform obj) {
        foreach(var i in spawnedTexts) {
            if(obj.gameObject.GetInstanceID() == i.gameObject.GetInstanceID()) {
                Destroy(i.gameObject);
                spawnedTexts.Remove(obj);
                return;
            }
        }
    }
}
