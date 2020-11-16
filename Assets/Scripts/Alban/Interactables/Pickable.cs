using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(RespawnObject)), RequireComponent(typeof(AudioSource))]
public class Pickable : MonoBehaviour, IInteractive
{
    [SerializeField] private InventoryItem _pickableItem = null;
    [Space]
    [SerializeField] private RespawnObject _respawner = null;
    [Space]
    [SerializeField] private ParticleSystem _particle = null;
    [SerializeField] private Rigidbody _rb = null;
    [Space, Header("FX Audio ID")]
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private string _pickUpID = string.Empty;
    [SerializeField] private string _inventoryAudioID = string.Empty;
    [SerializeField] private string _dropID = string.Empty;

    private MeshRenderer _meshRenderer = null;
    private bool _isHold = false;
    private Color _particleBaseColor = Color.white;

    public RespawnObject GetRespawner { get { return _respawner; } }
    public string GetInventoryAudioID { get { return _inventoryAudioID; } }
    public Rigidbody GetRigidbody { get { return _rb; } }

    private void Start()
    {
        _audioSource = this.GetComponent<AudioSource>();

        _rb = this.GetComponent<Rigidbody>();

        _meshRenderer = this.GetComponent<MeshRenderer>();

        if (_meshRenderer == null)
            _meshRenderer = this.GetComponentInChildren<MeshRenderer>();

        if(_respawner == null)
        {
            _respawner = this.GetComponent<RespawnObject>();
        }

        _respawner.SetLastValidPosition = this.transform.position;

        foreach (Material material in _meshRenderer.materials)
        {
            material.SetFloat("_Outline", 0f);
        }

        if (_particle != null)
        {
            _particleBaseColor = _particle.main.startColor.color;
            _particle.Stop();
        }
    }

    public InventoryItem GetItem()
    {
        return _pickableItem;
    }

    public void Enter(Transform parent)
    {
        _isHold = true;
        
        if(_particle != null)
        {
            ParticleSystem.MainModule main = _particle.main;

            main.startColor = new ParticleSystem.MinMaxGradient(Color.yellow);
            _particle.Stop();
        }

        this.transform.SetParent(parent);

        if (_pickUpID != string.Empty && _audioSource != null)
            _audioSource.PlayOneShot(SoundManager.Instance.GetAudio(_pickUpID));

        _rb.useGravity = false;
        _rb.isKinematic = true;
        _rb.detectCollisions = true;
    }

    public void Exit()
    {
        _isHold = false;

        if(_particle != null)
        {
            ParticleSystem.MainModule main = _particle.main;

            main.startColor = new ParticleSystem.MinMaxGradient(_particleBaseColor);
        }

        this.transform.SetParent(null);

        if (_dropID != string.Empty && _audioSource != null)
            _audioSource.PlayOneShot(SoundManager.Instance.GetAudio(_dropID));

        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.detectCollisions = true;
    }

    public void OnSeen()
    {
        if(_isHold == false)
        {
            if(_particle != null)
                _particle.Play();
        }
    }

    public void OnUnseen()
    {
        if(_particle != null)
            _particle.Stop();
    }
}