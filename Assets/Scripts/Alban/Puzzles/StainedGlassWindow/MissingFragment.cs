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

            _fragment.transform.position = _snapPosition.position;
            _fragment.transform.localPosition = _snapPosition.position;
            _fragment.transform.rotation = new Quaternion(0, 0, 0, 0);

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