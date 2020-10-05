using UnityEngine;

[CreateAssetMenu(fileName = "FP_InteractiveData", menuName = "FPData/Interactive")]
public class d_FPInteractive : ScriptableObject
{
    [SerializeField] private double _distance = 10.5f;
    [SerializeField] private LayerMask _interactable = 0;
    [SerializeField] private int _throwForce = 100;

    public double GetDistance { get { return _distance; } }
    public LayerMask GetInteractable { get { return _interactable; } }
    public int GetThrowForce { get { return _throwForce; } }
}