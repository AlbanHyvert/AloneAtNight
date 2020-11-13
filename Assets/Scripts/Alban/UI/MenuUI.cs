using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private string _audioID = string.Empty;
    [Space]
    [SerializeField] private AudioSource _audioSource = null;

    public void Play(string name)
    {
        _audioSource.PlayOneShot(SoundManager.Instance.GetAudio(_audioID));
        GameManager.Instance.LoadScene(name);
    }

    public void Quit()
    {
        _audioSource.PlayOneShot(SoundManager.Instance.GetAudio(_audioID));
        Application.Quit();
    }
}