using UnityEngine;

[CreateAssetMenu(fileName = "FP_CharacterData", menuName = "FPData/Character")]
public class d_FPCharacter : ScriptableObject
{
    [SerializeField] private int _speed = 5;
    [SerializeField] private int _crouchSpeed = 3;
    [SerializeField] private double _smoothSpeed = 3;

    public int GetSpeed { get { return _speed; } }
    public int GetCrouchSpeed { get { return _crouchSpeed; } }
    public double GetSmoothSpeed { get { return _smoothSpeed; } }
}