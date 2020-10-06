using Engine.Singleton;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private string _sceneName = string.Empty;

    private void Start()
    {
        SceneManager.LoadSceneAsync(_sceneName);
    }
}
