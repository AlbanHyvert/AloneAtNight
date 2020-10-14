using UnityEngine;

public class MissingFragment : MonoBehaviour
{
    [SerializeField] private E_FragmentIndex _missingIndex = E_FragmentIndex.PLACEMENT_1;
    [SerializeField] Vector3 _validRotation = Vector3.zero;
    [SerializeField] int _validScope = 10;
    
    private LayerMask _layer = 8;

    private bool _isValid = false;
    private Fragments _fragment = null;

    public bool GetIsValid { get { return _isValid; } }

    private void OnTriggerStay(Collider other)
    {
        LayerMask objectLayer = other.gameObject.layer;

        if (objectLayer == _layer)
        {
            if (_fragment == null)
            {
                _fragment = other.GetComponent<Fragments>();
            }
            else
            {
                if(_fragment.GetIndex == _missingIndex)
                {
                    Vector3 neededRot = new Vector3(_fragment.GetRotation.x, _fragment.GetRotation.y, _validRotation.z);

                    float dist = Vector3.Distance(_fragment.GetRotation, neededRot);

                    if (dist >= -_validScope && dist <= _validScope)
                    {
                        Debug.Log(_missingIndex.ToString() + " " + "is Valid");
                        _isValid = true;
                    }
                    else
                    {
                        _isValid = false;
                    }
                }
            }
        }

        if(_isValid == true)
        {
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _fragment = null;
    }
}