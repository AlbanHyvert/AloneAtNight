using UnityEngine;

[CreateAssetMenu(fileName = "FP_CameraData", menuName = "FPData/Camera")]
public class d_FPCamera : ScriptableObject
{
    [SerializeField] private int _rotationSpeed = 5;
    [SerializeField] private double _smoothSpeed = 2;
    [SerializeField] private int _minYRotation = 80;
    [SerializeField] private int _maxYRotation = -80;

    public int GetRotationSpeed { get { return _rotationSpeed; } }
    public double GetSmoothSpeed { get { return _smoothSpeed; } }
    public int GetMinYRotation { get { return _minYRotation; } }
    public int GetMaxYRotation { get { return _maxYRotation; } }
}