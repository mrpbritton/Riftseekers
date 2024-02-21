using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RocketLauncher : Attack {

    [SerializeField] GameObject rocketPreset;
    [SerializeField] float maxTravelDist, maxTravelTime, explosionSize, explosionDmg, explosionKnockback;
    public override void attack() {

        var curRocket = Instantiate(rocketPreset.gameObject);
        var rocketEndExplosion = explosionManager.queueExplode(curRocket.transform, explosionSize, explosionDmg, explosionKnockback, ExplosionManager.explosionState.HurtsEnemies, maxTravelTime);
        curRocket.GetComponent<RocketInstance>().setup(rocketEndExplosion, explosionManager, explosionSize, explosionDmg, explosionKnockback);
        var origin = transform.position + new Vector3(0f, 8f, 0f);
        curRocket.transform.position = origin;
        var d = InputManager.isUsingKeyboard() ? GetPoint() - origin : new Vector3(pInput.Player.ControllerAim.ReadValue<Vector2>().x, 0, pInput.Player.ControllerAim.ReadValue<Vector2>().y);
        if (d.x == 0 && d.z == 0)
            d = Vector3.forward;
        curRocket.transform.DOMove(origin + (d.normalized * maxTravelDist), maxTravelTime);
        Destroy(curRocket.gameObject, maxTravelTime + .1f);
    }

    public override void anim(Animator anim, bool reset)
    {
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
        return 3f / PlayerStats.CooldownMod;
    }
}
