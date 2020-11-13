using Engine.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private string _sceneName = string.Empty;
    [Space]
    [SerializeField] private LoadingUI _loadingUI = null;
    [Space]
    [SerializeField] private GameObject _hud = null;

    public string SceneName { get { return _sceneName; } set { _sceneName = value; } }
    public LoadingUI LoadingUI { get { return _loadingUI; } }

    private void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;

        DontDestroyOnLoad(_hud);

        LoadScene(SceneName);
    }

    public void LoadScene(string name)
    {
        _loadingUI.Enter();

        SceneManager.LoadSceneAsync(name);
    }

    private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.isLoaded)
        {
            _loadingUI.Exit();
        }
    }
}
