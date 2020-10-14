using System.Collections.Generic;
using UnityEngine;

public class GlassWindow : MonoBehaviour, IInteractive
{
    [SerializeField] private MissingFragment[] _fragments = null;
    [SerializeField] private Transform[] _fragmentsSpawnPos = null;
    [SerializeField] private Camera _camera = null;
    [Space]
    [SerializeField] private float _maxOutlineWidth = 0.10f;
    [SerializeField] private Color _outlineColor = Color.yellow;
    [SerializeField] private MeshRenderer _meshRenderer = null;
    
    private int _validIndex = 0;
    private List<bool> _isValidList = null;

    public Camera GetCamera { get { return _camera; } }

    public void Start() 
    {
        _isValidList = new List<bool>();

        _camera.gameObject.SetActive(false);

        if (_meshRenderer == null)
            _meshRenderer = this.GetComponent<MeshRenderer>();

        foreach (Material material in _meshRenderer.materials)
        {
            material.SetFloat("_Outline", 0f);
        }

        if (_fragments.Length > 0)
        {
            for (int i = 0; i < _fragments.Length; i++)
            {
                if (_fragments[i] != null)
                {
                    _isValidList.Add(_fragments[i].GetIsValid);
                }
            }

            GameLoopManager.Instance.UpdatePuzzles += Tick;
        }
        else
        {
            Debug.LogError("Missing Reference");
        }
    }

    public void Enter(Transform parent = null)
    {
        _camera.gameObject.SetActive(true);
        Debug.Log("Activated");
        //Activate Puzzle System
    }

    public void Exit()
    {
        Debug.Log("Desactivated");
        _camera.gameObject.SetActive(false);
        //Exit Puzzle System
    }

    void IInteractive.OnSeen()
    {
        if (_meshRenderer != null)
        {
            foreach (Material material in _meshRenderer.materials)
            {
                material.SetFloat("_Outline", _maxOutlineWidth);
                material.SetColor("_OutlineColor", _outlineColor);
            }
        }
    }

    void IInteractive.OnUnseen()
    {
        if (_meshRenderer != null)
        {
            foreach (Material material in _meshRenderer.materials)
            {
                material.SetFloat("_Outline", 0f);
            }
        }
    }

    private void Tick()
    {
        for (int i = 0; i < _fragments.Length; i++)
        {
            if(_fragments[i] != null)
            {
                if(_fragments[i].GetIsValid != _isValidList[i])
                {
                    _isValidList[i] = _fragments[i].GetIsValid;

                    if(_isValidList[i] == true)
                    {
                        _validIndex += 1;
                    }
                    else
                    {
                        if(_validIndex > 0)
                            _validIndex -= 1;
                    }
                }
            }
        }

        if(_validIndex >= _isValidList.Count)
        {
            Debug.Log("DONE");
        }
    }
}
