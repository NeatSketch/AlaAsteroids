using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab = default;
    [SerializeField] private Rect spawnRect = default;
    [SerializeField] private Rect movementTargetRect = default;

    public float Rate;

    private float lastSpawnTime = 0f;

    private void Update()
    {
        if (Time.time - lastSpawnTime >= 1f / Rate)
        {
            SpawnNewAsteroid();
            lastSpawnTime = Time.time;
        }
    }

    /// <summary>
    /// Spawn a new random-sized asteroid and push it in the direction of a random spot in the play area.
    /// </summary>
    private void SpawnNewAsteroid()
    {
        Vector3 position = GetRandomPositionOnRectPerimeter(spawnRect);
        Vector3 targetPosition = GetRandomPositionInRect(movementTargetRect);
        GameObject asteroidGO = Instantiate
        (
            asteroidPrefab,
            position,
            Quaternion.identity
        );
        Asteroid asteroid = asteroidGO.GetComponent<Asteroid>();
        asteroid.Init();
        asteroid.Push(targetPosition - position);
    }

    /// <summary>
    /// Get a random point on one of the sides of the rectangle with a uniform probability distribution across the perimeter.
    /// </summary>
    /// <param name="rect">The rectangle</param>
    /// <returns>A random point on the rectangle perimeter</returns>
    private Vector2 GetRandomPositionOnRectPerimeter(Rect rect)
    {
        float perimeter = 2f * (rect.width + rect.height);
        float widthToPerimeterRatio = rect.width / perimeter;
        float heightToPerimeterRatio = rect.height / perimeter;

        float randomFactor = Random.value;

        if (randomFactor < widthToPerimeterRatio)
        {
            float positionOnLine = Mathf.InverseLerp(0f, widthToPerimeterRatio, randomFactor);
            return Vector2.Lerp
            (
                new Vector2(rect.xMin, rect.yMin),
                new Vector2(rect.xMax, rect.yMin),
                positionOnLine
            );
        }

        if (randomFactor < 2f * widthToPerimeterRatio)
        {
            float positionOnLine = Mathf.InverseLerp(widthToPerimeterRatio, 2f * widthToPerimeterRatio, randomFactor);
            return Vector2.Lerp
            (
                new Vector2(rect.xMin, rect.yMax),
                new Vector2(rect.xMax, rect.yMax),
                positionOnLine
            );
        }

        if (randomFactor < 2f * widthToPerimeterRatio + heightToPerimeterRatio)
        {
            float positionOnLine = Mathf.InverseLerp(2f * widthToPerimeterRatio, 2f * widthToPerimeterRatio + heightToPerimeterRatio, randomFactor);
            return Vector2.Lerp
            (
                new Vector2(rect.xMin, rect.yMin),
                new Vector2(rect.xMin, rect.yMax),
                positionOnLine
            );
        }

        {
            float positionOnLine = Mathf.InverseLerp(2f * widthToPerimeterRatio + heightToPerimeterRatio, 1f, randomFactor);
            return Vector2.Lerp
            (
                new Vector2(rect.xMax, rect.yMin),
                new Vector2(rect.xMax, rect.yMax),
                positionOnLine
            );
        }
    }

    /// <summary>
    /// Get a random point inside the rectangle with a uniform probability distribution across the area.
    /// </summary>
    /// <param name="rect">The rectangle</param>
    /// <returns>A random point inside the rectangle</returns>
    private Vector2 GetRandomPositionInRect(Rect rect)
    {
        return new Vector2
        (
            Random.Range(rect.xMin, rect.xMax),
            Random.Range(rect.yMin, rect.yMax)
        );
    }
}
