using UnityEngine;

public class FP_Camera : MonoBehaviour
{
    [SerializeField] private Data _data;
    [SerializeField] private d_FPCamera _cameraData = null;

    private Vector3 _startPos = Vector3.zero;
    private float _currentX = 0;
    private float _currentY = 0;
    private float _rotationY = 0;
    private float _rotationX = 0;

    public Data GetData { get { return _data; } }
    public d_FPCamera GetCameraData { get { return _cameraData; } }

    [System.Serializable]
    public struct Data
    {
        public Camera camera;
        public HeadBobbing headBobbing;
        public Transform body;
        public Transform hand;
    }

    private void Start()
    {
        _rotationX = _data.body.localEulerAngles.y;

        InputManager.Instance.UpdateMouseXY += CameraRotation;
    }

    private void CameraRotation(float mouseX, float mouseY)
    {
        _rotationX = _data.body.localEulerAngles.y;

        mouseX *= _cameraData.GetRotationSpeed;
        mouseY *= _cameraData.GetRotationSpeed;

        _currentX = Mathf.Lerp(_currentX, mouseX, (float)_cameraData.GetSmoothSpeed * Time.deltaTime);
        _currentY = Mathf.Lerp(_currentY, mouseY, (float)_cameraData.GetSmoothSpeed * Time.deltaTime);

        _rotationX = _data.body.localEulerAngles.y + _currentX * (float)_cameraData.GetRotationSpeed;

        _rotationY += _currentY * (float)_cameraData.GetRotationSpeed;
        _rotationY = Mathf.Clamp(_rotationY, _cameraData.GetMinYRotation, _cameraData.GetMaxYRotation);

        _data.body.rotation = Quaternion.Euler(new Vector3(0, _rotationX, 0));
        _data.camera.transform.localRotation = Quaternion.Euler(-_rotationY, 0, 0);

        _rotationX = _data.body.localEulerAngles.y;
    }
}
