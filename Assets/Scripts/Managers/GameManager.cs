using Engine.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private string _sceneName = string.Empty;
    [Space]
    [SerializeField] private GameObject _hud = null;

    public string SceneName { get { return _sceneName; } set { _sceneName = value; } }

    private void Start()
    {
        DontDestroyOnLoad(_hud);

        SceneManager.LoadSceneAsync(_sceneName);
    }
}
