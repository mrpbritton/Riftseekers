using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RocketLauncher : Attack {

    [SerializeField] GameObject rocketPreset;
    [SerializeField] float maxTravelDist, maxTravelTime, explosionSize, explosionDmg, explosionKnockback;
    float height = 2f;

    public override void attack() {
        var curRocket = Instantiate(rocketPreset.gameObject);
        var rocketEndExplosion = explosionManager.queueExplode(curRocket.transform, explosionSize, explosionDmg, explosionKnockback, ExplosionManager.explosionState.HurtsEnemies, maxTravelTime);
        curRocket.GetComponentInChildren<RocketInstance>().setup(rocketEndExplosion, explosionManager, explosionSize, explosionDmg, explosionKnockback);
        var origin = transform.position + new Vector3(0f, height, 0f);
        curRocket.transform.position = origin;
        var d = InputManager.isUsingKeyboard() ? GetPoint() : new Vector3(pInput.Player.ControllerAim.ReadValue<Vector2>().x, 0, pInput.Player.ControllerAim.ReadValue<Vector2>().y);
        if (d.x == 0 && d.z == 0 && false)
            d = Vector3.forward;
        curRocket.transform.LookAt(d);
        var off = d - origin;
        off.y = 0f;
        curRocket.transform.localEulerAngles = new Vector3(0f, curRocket.transform.eulerAngles.y, curRocket.transform.eulerAngles.z);
        curRocket.transform.DOMove((off.normalized * maxTravelDist) + origin, maxTravelTime);
        Destroy(curRocket.gameObject, maxTravelTime + .1f);

        var dir = GetPoint();
        var direction = new Vector3(dir.x - transform.position.x + transform.localPosition.x,
                                        dir.y,
                                        dir.z - transform.position.z + transform.localPosition.z);
        FindObjectOfType<PlayerMovement>().slide(direction, -12f, .25f);
    }

    public override void anim(Animator anim, bool reset)
    {
    }
    public override void reset()
    {
    }
    public override attackType getAttackType() {
        return attackType.Special;
    }
    protected override float getDamage() {
        return 30f;
    }
    protected override float getCooldownTime() {
        return 3f / PlayerStats.CooldownMod;
    }
}
