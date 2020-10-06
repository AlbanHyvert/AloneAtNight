using UnityEngine;

[CreateAssetMenu(fileName = "DialogueAudio", menuName = "Datas/DialogueAudio")]
public class D_DialogueAudio : ScriptableObject
{
    [SerializeField] private DialAudio _audioSource;

    public DialAudio GetDial { get { return _audioSource; } }

    [System.Serializable]
    public struct DialAudio
    {
        public string id;
        public AudioClip clip;
        public int lifeTime;
    }
}
