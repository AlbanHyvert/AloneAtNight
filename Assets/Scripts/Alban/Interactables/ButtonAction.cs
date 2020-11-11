using cakeslice;
using UnityEngine;

public class ButtonAction : MonoBehaviour, IInteractive
{
    [SerializeField] private InventoryItem _objectItem = null;
    [SerializeField] private Transform _spawnPos = null;

    void IInteractive.Enter(Transform parent)
    {
        Debug.Log("Hi there");
        
        SpawnObject();

        Destroy(this.gameObject);

    }

    private void SpawnObject()
    {
       GameObject gameObject = Instantiate(_objectItem.GetPrefab(), _spawnPos.position, _objectItem.GetLocalRotation());
    }
    
    void IInteractive.Exit()
    {
        
    }

    void IInteractive.OnSeen()
    {

    }

    void IInteractive.OnUnseen()
    {
    }

    private void OnDestroy()
    {
        if(TryGetComponent(out Outline outline))
        {
            outline.DesactivateOutline();
        }
    }
}
