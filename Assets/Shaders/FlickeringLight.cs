using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    private bool bFlicker = false;
    public float timeDelay;
    public float minValue = 0.1f;
    public float maxValue = 1.0f;
    public float offTime = 0.1f;
    public float flickerIntensity = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bFlicker == false)
        {
            StartCoroutine(Flicker());
        }
    }
    IEnumerator Flicker()
    {
        bFlicker = true;
        //this.gameObject.GetComponent<Light>().enabled = false; //Disables the light
        this.gameObject.GetComponent<Light>().intensity = flickerIntensity; //lowers the light's intensity
        yield return new WaitForSeconds(offTime);
        this.gameObject.GetComponent<Light>().intensity = 10; //default intensity
        timeDelay = Random.Range(minValue, maxValue); //generates a random amount of time
        yield return new WaitForSeconds(timeDelay);
        bFlicker = false;
    }
}
