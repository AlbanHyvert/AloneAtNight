using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSpawn : MonoBehaviour
{
    [SerializeField] GameObject _partPrefab, _parentObject;
    [SerializeField] [Range(1, 1000)] int _lenght = 1;
    [SerializeField] float _partDistance = 0.21f;
    [SerializeField] bool _reset, _spawn, _snapFirst, _snapLast;

    void Update()
    {
        if (_reset)
        {
            foreach(GameObject tmp in GameObject.FindGameObjectsWithTag("Rope"))
            {
                Destroy(tmp);
            }
            _reset = false;
        }

        if (_spawn)
        {
            Spawn();

            _spawn = false;
        }
    }

    void Spawn()
    {
        int count = (int)(_lenght / _partDistance);

        for (int x = 0; x < count; x++)
        {
            GameObject tmp;

            tmp = Instantiate(_partPrefab, new Vector3(transform.position.x, transform.position.y + _partDistance * (x + 1), transform.position.z), Quaternion.identity, _parentObject.transform);
            tmp.transform.eulerAngles = new Vector3(180, 0, 0);

            tmp.name = _parentObject.transform.childCount.ToString();

            if (x == 0)
            {
                Destroy(tmp.GetComponent<CharacterJoint>());
                if (_snapFirst)
                {
                    tmp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }
            }
            else
            {
                tmp.GetComponent<CharacterJoint>().connectedBody = _parentObject.transform.Find((_parentObject.transform.childCount - 1).ToString()).GetComponent<Rigidbody>();
            }
        }

        if (_snapLast)
        {
            _parentObject.transform.Find((_parentObject.transform.childCount).ToString()).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
