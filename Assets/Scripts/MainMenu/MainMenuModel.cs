using System.Collections.Generic;
using UnityEngine;

public class MainMenuModel : MonoBehaviour
{
    [SerializeField] private List<ScriptableCartridge> cartridges;
    public List<ScriptableCartridge> Cartridges
    {
        get
        {
            if(cartridges == null)
            {
                cartridges = new List<ScriptableCartridge>();
            }

            return cartridges;
        }
    }

    [SerializeField] private GameObject cartridgePrefab;
    public GameObject CartridgePrefab
    {
        get
        {
            return cartridgePrefab;
        }
    }


    [SerializeField] private Camera mainCamera;
    public Camera MainCamera
    {
        get
        {
            if(mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            return mainCamera;
        }
    }

    [SerializeField] private Transform cartridgesCenter;
    public Transform CartridgesCenter
    {
        get
        {
            if(cartridgesCenter == null)
            {
                cartridgesCenter = GameObject.Find("GamesCenter").transform;
            }

            return cartridgesCenter;
        }
    }
}
