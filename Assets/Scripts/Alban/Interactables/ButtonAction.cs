using UnityEngine;

public class ButtonAction : MonoBehaviour, IInteractive
{
    [SerializeField] private ObjectItem _objectItem = null;
    [SerializeField] private Transform _spawnPos = null;

    void IInteractive.Enter(Transform parent)
    {
        Debug.Log("Hi there");
        SpawnObject();

        this.gameObject.layer = 0;

        Destroy(this);

    }

    private void SpawnObject()
    {
        Instantiate(_objectItem.GetPrefab(), _spawnPos);
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
}
