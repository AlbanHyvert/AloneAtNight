using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(AudioSource))]
public class CreateFood : MonoBehaviour, IInteractive
{
    [SerializeField] private InventoryItem food = null;
    [Space]
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private string _audioID = string.Empty;

    private bool _isStored = false;

    private void Start()
    {
        Rigidbody rigidbody = this.GetComponent<Rigidbody>();

        if (_audioSource == null)
            _audioSource = this.GetComponent<AudioSource>();

        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;

        _isStored = false;
    }

    public void Enter(Transform parent = null)
    {
        if (_isStored == false)
        {
            if (!string.IsNullOrEmpty(_audioID))
                _audioSource.PlayOneShot(SoundManager.Instance.GetAudio(_audioID));

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
