using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFPScene : MonoBehaviour
{
    [SerializeField] private string _sceneName;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("PROUT");
            GameManager.Instance?.LoadScene(_sceneName);
        }
    }
}
