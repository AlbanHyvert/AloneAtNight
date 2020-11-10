using UnityEngine;

public class UtensilCore : MonoBehaviour
{
    [SerializeField] private UtensilShadow[] _shadows = null;
    [Space]
    [SerializeField] private Animator _door = null;

    private int _index = 0;

    private void Start()
    {
        if(_shadows.Length > 1)
            GameLoopManager.Instance.UpdatePuzzles += Tick;
    }

    private void Tick()
    {
        for (int i = 0; i < _shadows.Length; i++)
        {
            if(_shadows[i] != null)
            {
                if (_shadows[i].HasTableware == true)
                {
                    _index++;
                    Destroy(_shadows[i]);
                }
            }
        }

        if (_index >= _shadows.Length)
        {
            _door.SetBool("IsActive", true);
            GameLoopManager.Instance.UpdatePuzzles -= Tick;
        }
    }
}
