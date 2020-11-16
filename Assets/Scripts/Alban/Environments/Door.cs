using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour, IInteractive
{
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private string _closedAudioID = string.Empty;

    public AudioSource AudioSource { get { return _audioSource; } }

    public void Enter(Transform parent = null)
    {
        if (!string.IsNullOrEmpty(_closedAudioID))
            _audioSource.PlayOneShot(SoundManager.Instance.GetAudio(_closedAudioID));
    }

    public void Exit()
    {
        
    }

    public void OnSeen()
    {
        
    }

    public void OnUnseen()
    {
        
    }
}
