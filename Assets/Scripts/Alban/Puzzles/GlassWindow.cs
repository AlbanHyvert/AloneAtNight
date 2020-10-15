using System.Collections.Generic;
using UnityEngine;

public class GlassWindow : MonoBehaviour, IInteractive
{
    [SerializeField] private MissingFragment[] _fragments = null;
    [SerializeField] private Transform[] _fragmentsSpawnPos = null;
    [SerializeField] private Camera _camera = null;
    
    private int _validIndex = 0;
    private List<bool> _isValidList = null;
    private List<Fragments> _fragmentList = null;
    private int _index = 0;
    private FP_Controller _player = null;

    public Camera GetCamera { get { return _camera; } }

    public void Start() 
    {
        _isValidList = new List<bool>();

        _camera.gameObject.SetActive(false);

        _fragmentList = new List<Fragments>();

        if (_fragments.Length > 0)
        {
            for (int i = 0; i < _fragments.Length; i++)
            {
                if (_fragments[i] != null)
                {
                    _fragments[i].Init(this);
                    _isValidList.Add(_fragments[i].GetIsValid);
                }
            }
        }
        else
        {
            Debug.LogError("Missing Reference");
        }

        Debug.Log("IsValid: " + _isValidList.Count + '\n' + "Fragments: " + _fragments.Length);
    }

    public void Enter(Transform parent = null)
    {
        _index = 0;

        _camera.gameObject.SetActive(true);

        _player = PlayerManager.Instance.GetPlayer;

        int itemCount = _player.GetInventory.GetPlayerItems().Count;

        if (itemCount > 0)
        {
            for (int i = 0; i < itemCount; i++)
            {
                InventoryItem item = _player.GetInventory.GetPlayerItems()[i];

                Fragments fragments = item.GetPrefab().GetComponent<Fragments>();

                if(fragments != null)
                {
                    GameObject gameObject = _player.CreateObjectInstance(item.GetObjectItem(), _fragmentsSpawnPos[_index]);

                    _index++;

                    fragments = gameObject.GetComponent<Fragments>();

                    fragments.GetComponent<Rigidbody>().isKinematic = true;
                    fragments.GetComponent<Rigidbody>().useGravity = false;

                    int rdmZ = Random.Range(10, 180);

                    Quaternion quaternion = fragments.transform.rotation;

                    quaternion.eulerAngles = new Vector3(0, 180, rdmZ);

                    fragments.transform.rotation = quaternion;

                    gameObject.AddComponent<DragObject>().Init(_camera);

                    _fragmentList.Add(fragments);
                }
            }
        }

        Debug.Log("Activated");

        GameLoopManager.Instance.UpdatePuzzles += Tick;
    }

    public void Exit()
    {
        if(_fragmentList.Count > 0)
        {
            for (int i = 0; i < _fragmentList.Count; i++)
            {
                GameObject fragments = _fragmentList[i].gameObject;

                if(fragments != null)
                    Destroy(fragments);
            }

            _fragmentList.Clear();
        }

        Debug.Log("Desactivated");
        _camera.gameObject.SetActive(false);

        GameLoopManager.Instance.UpdatePuzzles -= Tick;
    }

    void IInteractive.OnSeen()
    {
        
    }

    void IInteractive.OnUnseen()
    {

    }

    private void Tick()
    {
        for (int i = 0; i < _fragments.Length; i++)
        {
            if(_fragments[i] != null)
            {
                MissingFragment currentPlacementFragment = _fragments[i];
                Fragments currentFragment = currentPlacementFragment.GetFragment;

                if (currentPlacementFragment.GetIsValid != _isValidList[i])
                {
                    _isValidList[i] = currentPlacementFragment.GetIsValid;

                    if(_isValidList[i] == true)
                    {
                        _validIndex += 1;
                        _fragmentList.Remove(currentFragment);
                        RemoveFragment(currentFragment);
                    }
                }
            }
        }
        if (_validIndex >= _isValidList.Count)
        {
            //Activate Something
            Debug.Log("FINISHED");
            GameLoopManager.Instance.UpdatePuzzles -= Tick;
        }
    }

    public void RemoveFragment(Fragments fragments)
    {
        _player.RemoveFragment(fragments);
    }
}