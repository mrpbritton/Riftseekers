using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Bullet : MonoBehaviour
{
    [SerializeField, Tooltip("Speed of the projectile")]
    private float speed;
    [HideInInspector, Tooltip("Direction the projectile is headed")]
    public Vector3 direction;
    [Tooltip("Time in seconds it takes for bullet to die")]
    public float lifetime;
    Transform playerTrans;

    private void OnEnable()
    {
        playerTrans = FindObjectOfType<PlayerManager>().transform;
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        transform.position += Time.deltaTime * speed * direction.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Bullet"))
        {
            if (!other.gameObject.CompareTag("Enemy"))
            {
                AkSoundEngine.PostEvent("Object_Hit", gameObject);
            }
            var hitOffset = (transform.position - playerTrans.position).normalized * 1f;
            FindObjectOfType<ExplosionManager>().explodeWithColor(transform.position - hitOffset, .25f, ExplosionManager.explosionState.None, GetComponent<MeshRenderer>().material.color);
            Destroy(gameObject, 0.0001f);
        }
    }
}
