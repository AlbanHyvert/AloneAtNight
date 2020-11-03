using UnityEngine;

[CreateAssetMenu(fileName = "Movement Data", menuName = "Datas/FPController/Movement")]
public class D_FpController : ScriptableObject
{
    [SerializeField] private float _movingSpeed = 5;
    [SerializeField] private float _crouchSpeed = 2;
    [SerializeField] private float _smoothTime = 2;
    [SerializeField] private float _fallSpeed = 9.81f;

    public float MovingSpeed { get { return _movingSpeed; } set { _movingSpeed = value; } }
    public float CrouchSpeed { get { return _crouchSpeed; } set { _crouchSpeed = value; } }
    public float SmoothTime { get { return _smoothTime; } }
    public float FallSpeed { get { return _fallSpeed; } set { _fallSpeed = value; } }
}
