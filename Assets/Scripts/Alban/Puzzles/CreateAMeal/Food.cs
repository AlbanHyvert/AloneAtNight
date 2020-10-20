using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private E_AlimentIndex _index = E_AlimentIndex.NULL;

    public E_AlimentIndex GetIndex { get { return _index; } }
}