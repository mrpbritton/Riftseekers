using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : Singleton<BulletManager> {
    [SerializeField] GameObject bulletPref;
    int poolCount = 100;

    List<GameObject> bulletPool = new List<GameObject>();

    private void Start() {
        for(int i = 0; i < poolCount; i++) {
            var temp = Instantiate(bulletPref);
            temp.SetActive(false);

        }
    }

    public GameObject getBullet() {
        return bulletPool[0];
    }
}
