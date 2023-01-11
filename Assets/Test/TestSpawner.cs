using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spherePrefab;
    [SerializeField] private Transform sphereParent;
    [SerializeField] private Camera cam;

    [SerializeField] private float sphereSpawnDistance;
    [SerializeField] private int sphereNum;
    [SerializeField] private float rotationTime;

    private List<Transform> sphereList;

    private bool canRotate;
    private float rotationAngle;

    private void Start()
    {
        if(cam == null)
        {
            cam = Camera.main;
        }

        rotationAngle = 360.0f / sphereNum;
        sphereList = new List<Transform>();
        SpawnSphereAroundObject();

        canRotate = true;
    }


    private void Update()
    {
        if (!canRotate)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(RotateOverTime(sphereParent, Vector3.up, -rotationAngle, rotationTime));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(RotateOverTime(sphereParent, Vector3.up, rotationAngle, rotationTime));
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectGame();
        }
    }


    private void SelectGame()
    {
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo , Mathf.Infinity, LayerMask.GetMask("Default")))
        {
            Debug.Log(hitInfo.collider.gameObject.name);
        }
    }
    


    private IEnumerator RotateOverTime(Transform objectToRotate, Vector3 axis, float angle, float time)
    {
        canRotate = false;

        float startAngle = objectToRotate.rotation.eulerAngles.y;
        float endAngle = startAngle + angle;
        float currentTime = 0;

        while (currentTime < time)
        {
            float currentAngle = Mathf.Lerp(startAngle, endAngle, currentTime / time);
            objectToRotate.rotation = Quaternion.Euler(0f, currentAngle, 0f);
            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        objectToRotate.rotation = Quaternion.Euler(0, endAngle, 0);
        canRotate = true;
    }


    private void SpawnSphereAroundObject()
    {
        for (int i = 0; i < sphereNum; i++)
        {
            SpawnSphere(sphereParent.position, Vector3.back, rotationAngle * i);
        }
    }


    private void SpawnSphere(Vector3 targetPosition, Vector3 rotationAxis, float rotationAngle)
    {
        GameObject sphere = GameObject.Instantiate(spherePrefab, sphereParent);
        sphere.transform.position = targetPosition + rotationAxis * sphereSpawnDistance;
        sphere.transform.RotateAround(targetPosition, Vector3.up, rotationAngle);

        sphereList.Add(sphere.transform);
    }


    private void RotateSphere(Vector3 targetPosition, Transform sphere, Vector3 rotationAxis, float rotationAngle)
    {
        sphere.RotateAround(targetPosition, rotationAxis, rotationAngle);
    }
}
