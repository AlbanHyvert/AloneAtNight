using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Camera _camera = null;
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _rotation = 0.0f;
    [SerializeField] private float _clampMin = -20.0f;
    [SerializeField] private float _clampMax = 20.0f;
    [SerializeField] private Transform _target = null;
    [SerializeField] private float _fieldOfView = 0.0f;
    [SerializeField] private float _distanceToTarget = 0.0f;

    private void Start()
    {
        _fieldOfView = 60.0f;
    }

    private void Update()
    {
        transform.LookAt(_target);
        _rotation = transform.rotation.eulerAngles.y;

        //CLAMP
        /*_rotation = Mathf.Clamp(_rotation, _clampMin, _clampMax);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, _rotation, 0);*/






        _distanceToTarget = Vector3.Distance(_target.position, transform.position);
        _camera.fieldOfView = _fieldOfView;
    }

    private void OnGUI()
    {
        float max, min;
        max = 100.0f;
        min = 20.0f;
        _fieldOfView = GUI.HorizontalSlider(new Rect(20, 20, 100, 40), _fieldOfView, min, max);
    }
}
