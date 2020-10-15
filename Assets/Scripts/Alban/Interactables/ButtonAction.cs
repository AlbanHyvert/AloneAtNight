using UnityEngine;

public class ButtonAction : MonoBehaviour, IInteractive
{
    void IInteractive.Enter(Transform parent)
    {
        Debug.Log("Hi there");
    }

    void IInteractive.Exit()
    {
        
    }

    void IInteractive.OnSeen()
    {

    }

    void IInteractive.OnUnseen()
    {
    }
}
