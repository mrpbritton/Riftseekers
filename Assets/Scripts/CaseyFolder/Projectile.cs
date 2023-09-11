using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
//rotate model to 90 degrees on the x access
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Vector3 direction = new Vector3(0, 1, 0);
    [SerializeField]
    private float speed = 10;

    public Vector3 Direction { get { return direction; } set { direction = value; } }

    private void Start()
    {
        StartCoroutine(nameof(WaitToDie));
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    IEnumerator WaitToDie()
    {
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }

}
