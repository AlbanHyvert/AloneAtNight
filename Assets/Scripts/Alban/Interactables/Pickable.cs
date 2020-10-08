﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(RespawnObject))]
public class Pickable : MonoBehaviour, IInteractive
{
    [SerializeField] private ObjectItem _pickableItem = null;
    [Space]
    [SerializeField] private RespawnObject _respawner = null;
    [Space]
    [SerializeField] private float _maxOutlineWidth = 0.10f;
    [SerializeField] private Color _outlineColor = Color.yellow;
    [Space]
    [SerializeField] private ParticleSystem _particle = null;

    private MeshRenderer _meshRenderer = null;
    private Rigidbody _rb = null;
    private bool _isHold = false;
    private Color _particleBaseColor = Color.white;

    public RespawnObject GetRespawner { get { return _respawner; } }
    public Rigidbody GetRigidbody { get { return _rb; } }

    private void Start()
    {
        _pickableItem.SetObjectItem(_pickableItem);

        _rb = this.GetComponent<Rigidbody>();

        _meshRenderer = this.GetComponent<MeshRenderer>();

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

    public ObjectItem GetItem()
    {
        return _pickableItem;
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

        foreach (Material material in _meshRenderer.materials)
        {
            material.SetFloat("_Outline", 0f);
        }

        //_respawner.SetLastValidPosition = this.transform.position;

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
    }

    void IInteractive.OnSeen()
    {
        if(_isHold == false)
        {
            if(_particle != null)
                _particle.Play();

            if(_meshRenderer != null)
            {
                foreach (Material material in _meshRenderer.materials)
                {
                    material.SetFloat("_Outline", _maxOutlineWidth);
                    material.SetColor("_OutlineColor", _outlineColor);
                }
            }
        }
    }

    void IInteractive.OnUnseen()
    {
        if(_particle != null)
            _particle.Stop();

        if(_meshRenderer != null)
        {
            foreach (Material material in _meshRenderer.materials)
            {
                material.SetFloat("_Outline", 0f);
            }
        }
    }
}