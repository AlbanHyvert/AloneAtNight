using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CreateFood : MonoBehaviour, IInteractive
{
    [SerializeField] private ObjectItem food = null;

    private bool _isStored = false;

    private void Start()
    {
        Rigidbody rigidbody = this.GetComponent<Rigidbody>();

        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;

        _isStored = false;
    }

    public void Enter(Transform parent = null)
    {
        if (_isStored == false)
        {
            PlayerManager.Instance?.GetPlayer.GetInventory.AddItem(food, 1);
            _isStored = true;
        }
    }

    public void Exit()
    {  
    }
    public void OnSeen()
    {   
    }
    public void OnUnseen()
    {
    }
}
