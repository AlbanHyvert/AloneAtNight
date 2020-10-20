using UnityEngine;

public class Fragments : MonoBehaviour
{
    [SerializeField] private E_FragmentIndex _index = E_FragmentIndex.PLACEMENT_1;

    private bool _isSnap = false;

    public Vector3 GetRotation { get { return transform.eulerAngles; } }
    public E_FragmentIndex GetIndex { get { return _index; } }

    public bool GetIsSnap { get { return _isSnap; } }
    public bool SetIsSnap
    {
        set 
        { 
            _isSnap = value;

            DisableDrag(value);
        } 
    }

    public void DisableDrag(bool value)
    {
        if(value == true)
        {
            if(transform.TryGetComponent(out DragObject dragObject))
            {
                Destroy(dragObject);
            }
        }
    }
}