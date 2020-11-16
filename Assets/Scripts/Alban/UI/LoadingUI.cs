using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    public void Enter()
    {
        gameObject.SetActive(true);
    }

    public void Exit()
    {
        gameObject.SetActive(false);
    }
}