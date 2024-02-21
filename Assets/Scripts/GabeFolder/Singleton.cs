using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Object {
    public static T I { get; private set; }

    protected void Awake() {
        if(I == null) {
            I = FindObjectOfType<T>();
            // DontDestroyOnLoad(gameObject);
        }
        else {
            Debug.LogWarning($"{gameObject.name} destroyed.", this);
            // Destroy(gameObject);
        }
    }
}