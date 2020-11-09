using UnityEngine;

[CreateAssetMenu(fileName = "HeadBobbing Data", menuName = "Datas/FPController/HeadBobbing")]
public class D_FpHeadBobbing : ScriptableObject
{
    [SerializeField] private int _walkBobbingSpeed = 2;
    [SerializeField] private int _idleBobbingSpeed = 2;
    [SerializeField] private int _crouchBobbingSpeed = 2;
    [Space]
    [SerializeField] private float _walkbobbingAmount = 0.05f;
    [SerializeField] private float _idlebobbingAmount = 0.05f;
    [SerializeField] private float _crouchbobbingAmount = 0.05f;

    public int WalkBobbingSpeed { get { return _walkBobbingSpeed; } }
    public int IdleBobbingSpeed { get { return _idleBobbingSpeed; } }
    public int CrouchBobbingSpeed { get { return _crouchBobbingSpeed; } }
    public float WalkBobbingAmount { get { return _walkbobbingAmount; } }
    public float IdleBobbingAmount { get { return _idlebobbingAmount; } }
    public float CrouchBobbingAmount { get { return _crouchbobbingAmount; } }
}
