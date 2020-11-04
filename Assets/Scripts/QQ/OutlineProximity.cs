using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineProximity : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        transform.gameObject.GetComponent<Outline>().enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        transform.gameObject.GetComponent<Outline>().enabled = false;
    }
}
