using UnityEngine;

[CreateAssetMenu(fileName = "Audio File", menuName = "Sounds/Audio/FX")]
public class D_AudioClip : ScriptableObject
{
    [SerializeField] private string _id = string.Empty;
    [SerializeField] private AudioClip _clip = null;

    public string ID { get { return _id; } }
    public AudioClip Clip { get { return _clip; } }
}
