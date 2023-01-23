using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{

    #region SINGLETON

    private static LoadingManager instance;
    public static LoadingManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("Loading Manager");
                instance = go.AddComponent<LoadingManager>();
                DontDestroyOnLoad(go);
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    #endregion
    
    private const string PREFIX = "SCENE_";
    
    public AsyncOperation LoadingOperation { get; private set; }

    public void LoadScene(Enums.SceneName sceneName)
    {
        if (HasSceneAlreadyLoaded(sceneName))
        {
            Debug.LogWarning($"{sceneName} has already beem loaded");
            return;
        }

        SceneManager.LoadScene(PREFIX + sceneName.ToString());
    }

    public void LoadSceneAsync(Enums.SceneName sceneName)
    {
        if (HasSceneAlreadyLoaded(sceneName))
        {
            Debug.LogWarning($"{sceneName} has already beem loaded");
            return;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(PREFIX + Enums.SceneName.LoadingBar.ToString());
        operation.completed += asyncOperation =>
        {
            LoadingOperation = SceneManager.LoadSceneAsync(PREFIX + sceneName.ToString());
        };
    }



    public bool HasSceneAlreadyLoaded(Enums.SceneName sceneName)
    {
        Scene selectedScene = SceneManager.GetSceneByName(PREFIX + sceneName.ToString());
        return selectedScene.isLoaded;
    }

}
