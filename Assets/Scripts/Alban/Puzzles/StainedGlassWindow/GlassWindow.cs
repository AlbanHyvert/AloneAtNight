using System.Collections.Generic;
using UnityEngine;

public class GlassWindow : MonoBehaviour, IInteractive
{
    [SerializeField] private MissingFragment[] _fragments = null;
    [SerializeField] private Transform[] _fragmentsSpawnPos = null;
    [SerializeField] private Camera _camera = null;
    [Space]
    [SerializeField] private GameObject _light = null;
    [Space]
    [SerializeField] private Transform _spawnKeyPos = null;
    [SerializeField] private GameObject _key = null;
    [Space]
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private string _interactID = string.Empty;
    [SerializeField] private string _finishedID = string.Empty;

    private int _validIndex = 0;
    private int _index = 0;
    private List<bool> _isValidList = null;
    private List<Fragments> _fragmentList = null;
    private FP_Controller _player = null;

    public Camera GetCamera { get { return _camera; } }

    public void Start() 
    {
        if (_light != null)
            _light.SetActive(false);

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
    }

    public void Enter(Transform parent = null)
    {
        _index = 0;

        _camera.gameObject.SetActive(true);

        if (_interactID != string.Empty)
            _audioSource.PlayOneShot(SoundManager.Instance.GetAudio(_interactID));

        _player = PlayerManager.Instance.GetPlayersInstance.fpsPlayer;

        int itemCount = _player.GetInventory.GetInventories().Count;

        if (itemCount > 0)
        {
            for (int i = 0; i < itemCount; i++)
            {
                InventoryItem item = _player.GetInventory.GetInventories()[i];

                Fragments fragments = item.GetPrefab().GetComponent<Fragments>();

                if(fragments != null)
                {
                    GameObject gameObject = CreateObjectInstance(item, _fragmentsSpawnPos[_index]);

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

        _camera.gameObject.SetActive(false);

        GameLoopManager.Instance.UpdatePuzzles -= Tick;
    }

    void IInteractive.OnSeen()
    {
        
    }

    void IInteractive.OnUnseen()
    {

    }

    private GameObject CreateObjectInstance(InventoryItem objectItem, Transform anchor)
    {
        return Instantiate(objectItem.GetPrefab(), anchor.position, objectItem.GetLocalRotation());
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

        if (_key != null && _validIndex >= 7)
        {
            Instantiate(_key, _spawnKeyPos.position, _key.transform.rotation);
            _key = null;
        }

        if (_validIndex >= _isValidList.Count)
        {
            if (_interactID != string.Empty)
                _audioSource.PlayOneShot(SoundManager.Instance.GetAudio(_finishedID));

            if (_light != null)
                _light.SetActive(true);

            GameManager.Instance.LoadScene("EndGameScene");

            GameLoopManager.Instance.UpdatePuzzles -= Tick;
        }
    }

    public void RemoveFragment(Fragments fragments)
    {
        Pickable pickable = fragments.GetComponent<Pickable>();

        _player.GetInventory.RemoveItem(pickable.GetItem());
    }
}