using Engine.Singleton;
using System.Collections;
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

        _loadingUI.Enter();
        SceneManager.LoadSceneAsync(_sceneName);
    }

    public void LoadScene(string name)
    {
        _loadingUI.Enter();

        StartCoroutine(StartLoadingScene(name));
    }

    private IEnumerator StartLoadingScene(string name)
    {
        yield return new WaitForSecondsRealtime(2f);

        SceneManager.LoadSceneAsync(name);
    }

    private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.isLoaded)
        {
            _loadingUI.Exit();

            StopAllCoroutines();
        }
    }
}
