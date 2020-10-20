using UnityEngine;

public class MissingFragment : MonoBehaviour
{
    [SerializeField] private E_FragmentIndex _missingIndex = E_FragmentIndex.PLACEMENT_1;
    [SerializeField] Vector3 _validRotation = Vector3.zero;
    [SerializeField] int _validScope = 10;
    [Space]
    [SerializeField] private Transform _snapPosition = null;

    private LayerMask _layer = 8;
    private bool _isValid = false;
    private Fragments _fragment = null;
    private GlassWindow _soul = null;

    public bool GetIsValid { get { return _isValid; } }
    public Fragments GetFragment { get { return _fragment; } }

    public void Init(GlassWindow glassWindow)
    {
        _soul = glassWindow;
    }

    private void OnTriggerEnter(Collider other)
    {
        LayerMask objectLayer = other.gameObject.layer;

        if (_isValid == false && objectLayer == _layer)
        {
            if(other.TryGetComponent(out Fragments fragments))
            {
                if(fragments.GetIndex == _missingIndex)
                {
                    _fragment = fragments;

                    GameLoopManager.Instance.UpdatePuzzles += Tick;
                }
            }
        }
    }

    private void Tick()
    {
        Vector3 neededRot = new Vector3(_fragment.GetRotation.x, _fragment.GetRotation.y, _validRotation.z);

        float dist = Vector3.Distance(_fragment.GetRotation, neededRot);

        if (dist >= -_validScope && dist <= _validScope)
        {
            _isValid = true;

            _fragment.GetComponent<Collider>().enabled = false;

            _fragment.SetIsSnap = true;
        }

        if (_isValid == true)
        {
            _fragment.transform.SetParent(this.transform);

            _fragment.transform.position = Vector3.zero;
            _fragment.transform.localPosition = Vector3.zero;
            _fragment.transform.rotation = new Quaternion(0, 0, 0, 0);

            switch (_missingIndex)
            {
                case E_FragmentIndex.PLACEMENT_1:
                    Debug.Log("CUM");
                    break;
                case E_FragmentIndex.PLACEMENT_2:
                    Debug.Log("CUMON");
                    break;
                case E_FragmentIndex.PLACEMENT_3:
                    Debug.Log("CUMAS");
                    break;
                case E_FragmentIndex.PLACEMENT_4:
                    Debug.Log("CUMY");
                    break;
                case E_FragmentIndex.PLACEMENT_5:
                    Debug.Log("CUMRAW");
                    break;
                case E_FragmentIndex.PLACEMENT_6:
                    Debug.Log("CUMWHY");
                    break;
                case E_FragmentIndex.PLACEMENT_7:
                    Debug.Log("CUMWHERE");
                    break;
                case E_FragmentIndex.PLACEMENT_8:
                    Debug.Log("CUMWHAT");
                    break;
                case E_FragmentIndex.PLACEMENT_9:
                    Debug.Log("CUMWHO");
                    break;
                case E_FragmentIndex.PLACEMENT_10:
                    Debug.Log("CAM");
                    break;
            }

            GameLoopManager.Instance.UpdatePuzzles -= Tick;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(_fragment != null)
        {
            GameLoopManager.Instance.UpdatePuzzles -= Tick;
        }

        _fragment = null;
    }
}