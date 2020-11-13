using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;

    public void Enter()
    {
        gameObject.SetActive(true);
    }

    public void Exit()
    {
        gameObject.SetActive(false);
    }
}