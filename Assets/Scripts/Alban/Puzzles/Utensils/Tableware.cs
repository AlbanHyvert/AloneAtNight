using UnityEngine;

public class Tableware : MonoBehaviour
{
    [SerializeField] private E_Tableware _type = E_Tableware.FORK;

    public E_Tableware Type { get { return _type; } }
}
