using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Lookable : MonoBehaviour, IInteractive
{
    private Rigidbody _rb = null;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Enter(Transform parent = null)
    {
        _rb.useGravity = false;
        _rb.isKinematic = true;
        _rb.detectCollisions = true;
    }

    public void Exit()
    {
        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.detectCollisions = true;
    }

    public void OnSeen()
    {
        
    }

    public void OnUnseen()
    {
        
    }
}
