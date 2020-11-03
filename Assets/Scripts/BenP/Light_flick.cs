using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_flick : MonoBehaviour
{

    Light flickLight;
    public float minWaitTime;
    public float maxWaitTime;

    void Start()
    {
        flickLight = GetComponent<Light>();
        StartCoroutine(FlickeringLight());
    }

    IEnumerator FlickeringLight()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            flickLight.enabled = ! flickLight.enabled;
        }
    }
}
