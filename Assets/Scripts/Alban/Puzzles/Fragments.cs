using UnityEngine;

public class Fragments : MonoBehaviour
{
    [SerializeField] private E_FragmentIndex _index = E_FragmentIndex.PLACEMENT_1;

    public Vector3 GetRotation { get { return transform.eulerAngles; } }
    public E_FragmentIndex GetIndex { get { return _index; } }
}