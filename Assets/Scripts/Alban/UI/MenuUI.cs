using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public void Play(string name)
    {
        GameManager.Instance.LoadScene(name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}