using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SowrdDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.GetMask("Enemy"))
            col.gameObject.GetComponent<Health>().takeDamage(5);
    }
}
