using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CreateFood : MonoBehaviour, IInteractive
{
    [SerializeField] private InventoryItem food = null;

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
            PlayerManager.Instance.GetPlayersInstance.fpsPlayer.GetInventory.AddItem(food);
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
