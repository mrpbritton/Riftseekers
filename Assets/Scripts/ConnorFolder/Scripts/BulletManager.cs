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
            bulletPool.Add(temp);
        }
    }

    public GameObject getBullet() {
        var temp = bulletPool[0];
        temp.SetActive(true);
        bulletPool.RemoveAt(0);
        return temp;
    }

    public void repoolBullet(GameObject b) {
        b.SetActive(false);
        bulletPool.Add(b);
    }
}
