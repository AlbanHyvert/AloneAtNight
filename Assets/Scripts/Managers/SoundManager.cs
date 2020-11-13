using System.Collections.Generic;
using UnityEngine;
using Engine.Singleton;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private D_AudioClip[] _audioClips = null;

    private Dictionary<string, AudioClip> _fxSounds = null;

    private void Start()
    {
        _fxSounds = new Dictionary<string, AudioClip>();

        for (int i = 0; i < _audioClips.Length; i++)
        {
            if (_audioClips[i] == null)
                return;

            if(_audioClips[i].ID != string.Empty)
                _fxSounds.Add(_audioClips[i].ID, _audioClips[i].Clip);
        }
    }

    public AudioClip GetAudio(string id)
       => _fxSounds[id];
}
