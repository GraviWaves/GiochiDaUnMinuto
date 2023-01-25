using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuPresenter : MonoBehaviour
{
    [SerializeField] MainMenuModel model;

    [SerializeField] private float cartridgeSpawnDistance;
    [SerializeField] private float animationTimeSeconds;
    [SerializeField] private float gameSelectRaycastDistance;
    [SerializeField] private float changeSceneDelay;

    [SerializeField] private AudioClip mainMenuBgm;
    [SerializeField] private AudioClip switchGameSe;
    [SerializeField] private AudioClip SelectGameSe;

    private float rotationAngle;
    private bool canRotate = true;

    private const float ANGLE_FULL_CIRCLE = 360.0f;

    void Start()
    {
        rotationAngle = ANGLE_FULL_CIRCLE / model.Cartridges.Count;
        SpawnCartridgesAroundCenter();
        AudioManager.Instance.PlayBgm(mainMenuBgm, true);
    }

    void Update()
    {
        if (!canRotate)
        {
            return;
        }

        bool RightInput = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        bool LeftInput = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);

        if (RightInput)
        {
            StartCoroutine(RotateOverTime(model.CartridgesCenter, Vector3.up, rotationAngle, animationTimeSeconds));
            AudioManager.Instance.PlaySeOneShot(switchGameSe);
        }
        else if (LeftInput)
        {
            StartCoroutine(RotateOverTime(model.CartridgesCenter, Vector3.up, -rotationAngle, animationTimeSeconds));
            AudioManager.Instance.PlaySeOneShot(switchGameSe);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectGame();
        }
    }


    private void SelectGame()
    {
        if(Physics.Raycast(model.MainCamera.transform.position, model.MainCamera.transform.forward, out RaycastHit hitInfo, gameSelectRaycastDistance))
        {
            CartridgeVisualizer visualizer = hitInfo.collider.gameObject.GetComponent<CartridgeVisualizer>();
            AudioManager.Instance.PlaySeOneShot(SelectGameSe);
            StartCoroutine(ChangeScene(visualizer.Info.GameScene));
        }
    }

    private IEnumerator ChangeScene(Enums.SceneName sceneName)
    {
        float targetVolume = 0f;
        AudioManager.Instance.FadeBgmVolume(targetVolume, changeSceneDelay);

        yield return new WaitForSeconds(changeSceneDelay);

        LoadingManager.Instance.LoadSceneAsync(sceneName);
    }


    private IEnumerator RotateOverTime(Transform objectToRotate, Vector3 rotationAxis, float angle, float time)
    {
        canRotate = false;

        float startAngle = objectToRotate.rotation.eulerAngles.y;
        float endAngle = startAngle + angle;
        float currentTime = 0f;

        while(currentTime < time)
        {
            float currentAngle = Mathf.Lerp(startAngle, endAngle, currentTime / time);
            objectToRotate.rotation = Quaternion.Euler(0f, currentAngle, 0f);
            currentTime += Time.deltaTime;
            yield return null;
        }

        objectToRotate.rotation = Quaternion.Euler(0f, endAngle, 0f);
        
        canRotate = true;
    }


    private void SpawnCartridgesAroundCenter()
    {
        for (int i = 0; i < model.Cartridges.Count; i++)
        {
            SpawnCartridge(Vector3.forward, rotationAngle, i, model.Cartridges[i]);
        }
    }


    private void SpawnCartridge(Vector3 spawnDistanceAxis, float rotationAngle, int rotationMultiplier, ScriptableCartridge info)
    {
        GameObject cartridge = GameObject.Instantiate(model.CartridgePrefab, model.CartridgesCenter);
        cartridge.transform.position = model.CartridgesCenter.position + (spawnDistanceAxis * cartridgeSpawnDistance);
        cartridge.transform.RotateAround(model.CartridgesCenter.position, Vector3.up, rotationAngle * rotationMultiplier);

        CartridgeVisualizer visualizer = cartridge.GetComponent<CartridgeVisualizer>();
        visualizer.SetInfo(info);
    }
}
