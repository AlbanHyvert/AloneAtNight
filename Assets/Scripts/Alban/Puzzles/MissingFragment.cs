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
                        _isValid = true;
                    }
                    else
                    {
                        _isValid = false;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == _fragment)
        {
            _fragment = null;
        }
    }
}