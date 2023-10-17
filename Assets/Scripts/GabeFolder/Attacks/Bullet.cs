using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Bullet : MonoBehaviour
{
    [SerializeField, Tooltip("Speed of the projectile")]
    private float speed;
    [Tooltip("Direction the projectile is headed")]
    public Vector3 direction;
    [Tooltip("Damage of the projectile")]
    public float damage;
    [Tooltip("Time in seconds it takes for bullet to die")]
    public float lifetime;

    private void OnEnable()
    {
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        transform.position += Time.deltaTime * speed * direction.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player"))
        {
            if (other.TryGetComponent(out EnemyHealth enemy))
            {
                enemy.damageTaken(damage);
            }
            Destroy(gameObject);
        }
    }
}
