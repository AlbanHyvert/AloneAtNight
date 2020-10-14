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
    private List<Fragments> _fragmentList = null;
    private int _index = 0;

    public Camera GetCamera { get { return _camera; } }

    public void Start() 
    {
        _isValidList = new List<bool>();

        _camera.gameObject.SetActive(false);

        _fragmentList = new List<Fragments>();

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
        _index = 0;

        _camera.gameObject.SetActive(true);

        FP_Controller fP_Controller = PlayerManager.Instance.GetPlayer;

        int itemCount = fP_Controller.GetInventory.GetPlayerItems().Count;

        if (itemCount > 0)
        {
            for (int i = 0; i < itemCount; i++)
            {
                InventoryItem item = fP_Controller.GetInventory.GetPlayerItems()[i];

                Fragments fragments = item.GetPrefab().GetComponent<Fragments>();

                if(fragments != null)
                {
                    GameObject gameObject = fP_Controller.CreateObjectInstance(item.GetObjectItem(), _fragmentsSpawnPos[_index]);

                    _index++;

                    fragments = gameObject.GetComponent<Fragments>();

                    fragments.GetComponent<Rigidbody>().isKinematic = true;
                    fragments.GetComponent<Rigidbody>().useGravity = false;

                    gameObject.AddComponent<DragObject>().Init(_camera);

                    _fragmentList.Add(fragments);
                }
            }
        }

        Debug.Log("Activated");
        //Activate Puzzle System
    }

    public void Exit()
    {
        if(_fragmentList.Count > 0)
        {
            for (int i = 0; i < _fragmentList.Count; i++)
            {
                Fragments fragments = _fragmentList[i];

                Destroy(fragments.gameObject);

                _fragmentList.Remove(fragments);
            }
        }

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
