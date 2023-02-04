using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region SINGLETON

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("Game Manager");
                instance = go.AddComponent<GameManager>();
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

    public Camera MainCamera { set; get; }



    public Vector2 GetCamera2DBounds()
    {
        return new Vector2(
            MainCamera.orthographicSize * MainCamera.aspect,
            MainCamera.orthographicSize
            );
    }
}
