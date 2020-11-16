using cakeslice;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonAction : MonoBehaviour, IInteractive
{
    [SerializeField] private InventoryItem _objectItem = null;
    [SerializeField] private Transform _spawnPos = null;
    [Space]
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private string _audioID = string.Empty;
    
    void IInteractive.Enter(Transform parent)
    {
        if (_audioID != string.Empty)
            _audioSource.PlayOneShot(SoundManager.Instance.GetAudio(_audioID));

        SpawnObject();

        this.gameObject.SetActive(false);

        Destroy(this);
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
