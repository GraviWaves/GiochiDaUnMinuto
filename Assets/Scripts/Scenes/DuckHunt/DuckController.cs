using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DuckController : MonoBehaviour, IPointerDownHandler
{
    // Duck animation status 
    public enum DuckStatus
    {
        Standby,
        Flyng,
        Falling
    }

    //Constant
    private const float ZER0_SPEED = 0;
    private const float SPAWN_ANGLE_MIN = 45;
    private const float SPAWN_ANGLE_MAX = 80;
    private const float FALL_SPEED = 7f;
    private const float DUCK_FALL_DELAY = 1f;

    //Instantiation time position
    private Vector3 initialPosition;

    //Renderers
    private SpriteRenderer renderer;
    private SpriteRenderer grassRenderer;

    // Duck speed
    private float speed;

    //Status
    public DuckStatus Status { get; private set; }

    private void Awake()
    {
        Status = DuckStatus.Standby;
        initialPosition = transform.position;
        renderer = GetComponent<SpriteRenderer>();
    }

    
    /// <summary>
    /// Spawn duck and let it fly
    /// </summary>
    /// <param name="duckSpeed">duck fly speed</param>
    /// <param name="spawnPosition">duck initial spawn position</param>
    /// <param name="grass">grass sprite renderer</param>
    public void ReleaseDuck(float duckSpeed, Vector3 spawnPosition, SpriteRenderer grass)
    {
        speed = duckSpeed;
        transform.position = spawnPosition;
        grassRenderer = grass;

        Status = DuckStatus.Flyng;
        StartCoroutine(DuckFlying());
    }


    /// <summary>
    /// Duck movements
    /// </summary>
    private IEnumerator DuckFlying()
    {
        //Set camera and sprite bounds
        Vector2 cameraBounds = GameManager.Instance.GetCamera2DBounds();
        Vector3 spriteSize = renderer.sprite.bounds.size;

        //Set duck fly direction
        Vector3 direction = GetDuckDirection();

        //Duck fly zone
        Vector2 flyZone = new Vector2(cameraBounds.x - spriteSize.x, cameraBounds.y - spriteSize.y);
        
        //When the duck goes out from the grass, set a new bounce poisition
        float terrainBoucepositionY = transform.position.y;
        StartCoroutine(ResetSpawnPosition(terrainBoucepositionY, spriteSize.y, (newPosition) =>
        {
            terrainBoucepositionY = newPosition;
        }));


        while (Status == DuckStatus.Flyng)
        {
            if(transform.position.x > flyZone.x || transform.position.x < -flyZone.x)
            {
                direction = new Vector3(-direction.x, direction.y);
            }

            if (transform.position.y > flyZone.y || transform.position.y < terrainBoucepositionY)
            {
                direction = new Vector3(direction.x, -direction.y);
            }

            transform.position += direction * speed * Time.deltaTime;

            yield return null;  
        }
    }


    /// <summary>
    /// Shooted duck fall movement
    /// </summary>
    private IEnumerator DuckFaling()
    {
        float hidePosition = grassRenderer.transform.position.y - grassRenderer.size.y;

        yield return new WaitForSeconds(DUCK_FALL_DELAY);

        while(Status == DuckStatus.Falling)
        {
            if(transform.position.y < hidePosition)
            {
                break;
            }

            transform.position += Vector3.down * FALL_SPEED * Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }




    /// <summary>
    /// When the duck spawn and goes out from the grass, than change the bounce check height
    /// </summary>
    /// <param name="spawnPositionY">duck spawn position</param>
    /// <param name="spriteDou8oundsY">duck sprite height</param>
    /// <param name="callback">check coordinates new poisition callback</param>
    private IEnumerator ResetSpawnPosition(float spawnPositionY, float spriteDou8oundsY, Action<float> callback)
    {
        while(transform.position.y - spriteDou8oundsY < spawnPositionY + grassRenderer.size.y)
        {
            yield return null;
        }

        callback.Invoke(transform.position.y);
    }

    /// <summary>
    /// Get duck initial direction
    /// </summary>
    /// <returns>Vector3 direction</returns>
    private Vector3 GetDuckDirection()
    {
        //if the random number is even: -1, otherwise 1
        int direction = (UnityEngine.Random.Range(1, 3) & 1) == 1 ? -1 : 1;
        
        //Get a random angle 
        float angle = UnityEngine.Random.Range(SPAWN_ANGLE_MIN, SPAWN_ANGLE_MAX);
        return (Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right).normalized * direction;
    }

    private bool IsDuckShot(Vector2 pointerPosition)
    {

        Vector3 localPointerPosition = transform.InverseTransformPoint(new Vector3(pointerPosition.x, pointerPosition.y, 0));
        Vector3 localSpriteExtents = renderer.sprite.bounds.extents;

        localPointerPosition = localPointerPosition + localSpriteExtents;
        localPointerPosition.x /= localSpriteExtents.x * 2;
        localPointerPosition.y /= localSpriteExtents.y * 2;

        Color shotedColor = renderer.sprite.texture.GetPixel((int)pointerPosition.x, (int)pointerPosition.y);

        if(shotedColor.grayscale != 0)
        {
            return true;
        }

        return false;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if(Status == DuckStatus.Falling)
        {
            return;
        }


        //if(!IsDuckShot(eventData.pressPosition))
        //{
        //    return;
        //}


        Status = DuckStatus.Falling;
        StartCoroutine(DuckFaling());
    }

    private void OnDisable()
    {
        Status = DuckStatus.Standby;
        transform.position = initialPosition;
        speed = ZER0_SPEED;
    }

    
}
