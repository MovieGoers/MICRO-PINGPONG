using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using scenemanager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;

    public static SceneManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance)
        {
            Destroy(instance);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {

    }
    public void StartGame()
    {
    }
    public void LoadScene(string sceneName)
    {
        scenemanager.LoadSceneAsync(sceneName);
    }

    public void RestartGame()
    {
        scenemanager.LoadScene(scenemanager.GetActiveScene().buildIndex);
    }

    public void HandleGameOver()
    {
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetScene()
    {
        scenemanager.LoadSceneAsync(scenemanager.GetActiveScene().buildIndex);
    }
}
