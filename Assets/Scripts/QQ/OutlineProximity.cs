using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using quickOutline;

public class OutlineProximity : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.gameObject.GetComponent<Outline>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.gameObject.GetComponent<Outline>().enabled = false;
        }
    }
}
