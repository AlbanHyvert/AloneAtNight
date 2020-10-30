using UnityEngine;

[CreateAssetMenu(fileName = "HeadBobbing Data", menuName = "Datas/FPController/HeadBobbing")]
public class D_FpHeadBobbing : ScriptableObject
{
    [SerializeField] private int _walkBobbingSpeed = 2;
    [SerializeField] private float _bobbingAmount = 0.05f;

    public int WalkBobbingSpeed { get { return _walkBobbingSpeed; } }
    public float BobbingAmount { get { return _bobbingAmount; } }
}
