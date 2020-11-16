using UnityEngine;

public class UtensilCore : MonoBehaviour
{
    [SerializeField] private UtensilShadow[] _shadows = null;
    [Space]
    [SerializeField] private Animator _doorAnim = null;
    [SerializeField] private Door _door = null;
    [Space]
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private string _audioFinishedID = string.Empty;

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
            if (!string.IsNullOrEmpty(_audioFinishedID))
                _door.AudioSource.PlayOneShot(SoundManager.Instance.GetAudio(_audioFinishedID));

            _doorAnim.SetBool("IsActive", true);
            GameLoopManager.Instance.UpdatePuzzles -= Tick;
        }
    }
}
