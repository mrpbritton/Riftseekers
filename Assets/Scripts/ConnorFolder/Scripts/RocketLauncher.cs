using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RocketLauncher : Attack {

    [SerializeField] GameObject rocketPreset;
    [SerializeField] float maxTravelDist, maxTravelTime, explosionSize;

    public override void attack() {
        var curRocket = Instantiate(rocketPreset.gameObject);
        var rocketEndExplosion = explosionManager.queueExplode(curRocket.transform, explosionSize, ExplosionManager.explosionState.HurtsEnemies, maxTravelTime);
        curRocket.GetComponent<RocketInstance>().setup(rocketEndExplosion, explosionManager, explosionSize);
        curRocket.transform.position = transform.position;
        var dir = GetPoint() - transform.position;
        curRocket.transform.DOMove(transform.position + (dir.normalized * maxTravelDist), maxTravelTime);
        Destroy(curRocket.gameObject, maxTravelTime + .1f);
    }

    public override void reset()
    {
    }
    public override attackType getAttackType() {
        return attackType.QAbility;
    }
    protected override float getDamage() {
        return 30f;
    }
    protected override float getCooldownTime() {
        return 3f;
    }
}
