using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickable : MonoBehaviour, IInteractive
{
    [SerializeField] private ParticleSystem _particle = null;

    private Rigidbody _rb = null;
    private Vector3 _lastValidPosition = Vector3.zero;
    private bool _isHold = false;
    private Color _particleBaseColor = Color.white;

    public Vector3 GetLastValidPos { get { return _lastValidPosition; } }
    public Rigidbody GetRigidbody { get { return _rb; } }

    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();

        _lastValidPosition = this.transform.localPosition;

        if (_particle != null)
        {
            _particleBaseColor = _particle.main.startColor.color;
            _particle.Stop();
        }
    }

    void IInteractive.Enter(Transform parent)
    {
        _isHold = true;
        
        if(_particle != null)
        {
            ParticleSystem.MainModule main = _particle.main;

            main.startColor = new ParticleSystem.MinMaxGradient(Color.yellow);
            _particle.Stop();
        }

        _lastValidPosition = this.transform.localPosition;

        this.transform.SetParent(parent);

        _rb.useGravity = false;
        _rb.isKinematic = true;
        _rb.detectCollisions = true;
    }

    void IInteractive.Exit()
    {
        _isHold = false;

        if(_particle != null)
        {
            ParticleSystem.MainModule main = _particle.main;

            main.startColor = new ParticleSystem.MinMaxGradient(_particleBaseColor);
        }

        this.transform.SetParent(null);

        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.detectCollisions = true;

        _lastValidPosition = this.transform.localPosition;
    }

    void IInteractive.OnSeen()
    {
        if(_isHold == false)
        {
            if(_particle != null)
                _particle.Play();
        }
    }

    void IInteractive.OnUnseen()
    {
        if(_particle != null)
            _particle.Stop();
    }
}