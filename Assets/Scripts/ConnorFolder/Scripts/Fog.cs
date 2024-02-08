using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Fog : MonoBehaviour {
    [SerializeField] SpriteRenderer fog;
    [SerializeField] int res, distToClear;
    List<Vector2Int> activeCoords = new List<Vector2Int>();

    Transform playerTrans;

    Texture2D texture;

    Vector3 pStart;

    private void Start() {
        if(SceneManager.GetActiveScene().name == "The Hub")
            Destroy(gameObject);
        playerTrans = FindObjectOfType<PlayerMovement>().transform;
        Texture2D tex = new Texture2D(res, res);
        for(int x = 0; x < res; x++) {
            for(int y = 0; y < res; y++) {
                tex.SetPixel(x, y, Color.grey);
                activeCoords.Add(new Vector2Int(x, y));
            }
        }
        tex.Apply();
        Sprite s = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), Vector3.zero);
        var scale = fog.transform.localScale / 4f;
        var p = playerTrans.position - new Vector3(scale.x, 0f, scale.z);
        fog.sprite = s;
        texture = tex;

        pStart = playerTrans.position;
    }

    private void LateUpdate() {
        clearFog(distToClear, getPlayerPoint());
    }

    void clearFog(int radius, Vector2Int point) {
        for(int x = point.x - radius / 2; x < point.x + radius / 2; x++) {
            for(int y =  point.y - radius / 2; y < point.y + radius / 2; y++) {
                texture.SetPixel(x, y, Color.clear);
            }
        }
        texture.Apply();
    }

    Vector2Int getPlayerPoint() {
        var s = fog.transform.localScale.x / 2f;
        var step = s / res;
        var offset = playerTrans.position - pStart;
        offset /= 4f;
        return new Vector2Int((res / 2) + (int)(offset.x / step), (res / 2) + (int)(offset.z / step));
    }
    Vector2Int getWorldPoint(Vector3 pos) {
        var s = fog.transform.localScale.x / 2f;
        var step = s / res;
        var offset = pos - pStart;
        offset /= 4f;
        return new Vector2Int((res / 2) + (int)(offset.x / step), (res / 2) + (int)(offset.z / step));
    }
}
