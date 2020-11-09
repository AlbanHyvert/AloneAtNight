using UnityEngine;

[CreateAssetMenu(fileName = "Camera Data", menuName = "Datas/FPController/Camera")]
public class D_FpCamera : ScriptableObject
{
    [SerializeField] private float _rotationSpeed = 10;
    [SerializeField] private int _maxXRotation = -70;
    [SerializeField] private int _minXRotation = 70;
    [SerializeField] private float _smoothTime = 2;
    
    public float RotationSpeed { get { return _rotationSpeed; } set { _rotationSpeed = value; } }
    public int MaxXRotation { get { return _maxXRotation; } }
    public int MinXRotation { get { return _minXRotation; } }
    public float SmoothTime { get { return _smoothTime; } }
}