using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Push");
            transform.position -= new Vector3(1, 0, 0);
        }

        else if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Pull");
            transform.position += new Vector3(1, 0, 0);
        }
    }
}
