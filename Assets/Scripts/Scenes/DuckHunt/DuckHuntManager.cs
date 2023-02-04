using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckHuntManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject[] duckPrefabs;
    [SerializeField] private GameObject ducksParent;
    [SerializeField] private SpriteRenderer grass;
    
    [SerializeField] private float duckInitialSpeed;
    [SerializeField] private float gameStartTimerSeconds;
    [SerializeField] private int levelMaxDuckCount;
    [SerializeField] private int stageMaxCount;
    
    private int currentDuckCount;
    private int currentStage;

    private Queue<DuckController> ducks = new Queue<DuckController>();



    private void Awake()
    {
        GameManager.Instance.MainCamera = mainCamera;

        SetDucksPoolPosition();

        int prefabNum = 0;
        for (int i = 0; i < Definition.DuckHuntMaxDuck; i++)
        {
            prefabNum = prefabNum < duckPrefabs.Length ? prefabNum : 0;
            GameObject duck = SpawnDuck(prefabNum);
            ducks.Enqueue(duck.GetComponent<DuckController>());

            prefabNum++;
        }

        currentDuckCount = 0;
        currentStage = 0;
    }

    void Start()
    {
        StartCoroutine(StartGame());    
    }


    /// <summary>
    /// Duck hunt game system start
    /// </summary>
    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(gameStartTimerSeconds);

        float ducksSpeed = duckInitialSpeed;

        while(currentStage < stageMaxCount)
        {
            while (currentDuckCount < levelMaxDuckCount)
            {
                DuckController duck = ducks.Dequeue();
                duck.gameObject.SetActive(true);

                //Spawn duck to a randome point behind the grass
                duck.ReleaseDuck(ducksSpeed, GetRandomSpawnPoint(duck), grass);

                // wait while the duck GameObject is active 
                while (duck.gameObject.activeSelf)
                {
                    yield return null;
                }

                ducks.Enqueue(duck);

                //Go to the next duck
                currentDuckCount++;
            }

            ducksSpeed += 5f;
            currentDuckCount = 0;
            yield return new WaitForSeconds(gameStartTimerSeconds);
        }
        
    }


    /// <summary>
    /// Get duck random spawn point
    /// </summary>
    /// <param name="duck">Duck script</param>
    /// <returns>Random spawn point</returns>
    private Vector3 GetRandomSpawnPoint(DuckController duck)
    {
        SpriteRenderer renderer = duck.GetComponent<SpriteRenderer>();

        float grassY = grass.transform.position.y;

        //Calculate camera size
        float screenWidth = GameManager.Instance.GetCamera2DBounds().x;

        //Calculate duck bounds 
        float duckBounds = renderer.sprite.bounds.size.x;
        float spawnBounds = screenWidth - duckBounds;

        return new Vector3(UnityEngine.Random.Range(-spawnBounds, spawnBounds), grassY, 0);
    }


    /// <summary>
    /// Set Duck information and spawn it
    /// </summary>
    /// <param name="prefabNum">Duck prefab</param>
    /// <returns>Duck game object</returns>
    private GameObject SpawnDuck(int prefabNum)
    {
        var obj = GameObject.Instantiate(duckPrefabs[prefabNum], ducksParent.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.name = "Duck";
        obj.SetActive(false);

        return obj;
    }


    /// <summary>
    /// Set duck pool parent poisition in the scene
    /// </summary>
    private void SetDucksPoolPosition()
    {
        float height = mainCamera.orthographicSize;
        float width = height * Screen.width / Screen.height;
        float distanceMultiplier = 2;

        ducksParent.transform.position = new Vector3(width * distanceMultiplier, 
                                                     height * distanceMultiplier, 
                                                     0);
    }

}
