using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    [SerializeField] private float minHeight = default;
    [SerializeField] private float minRadius = default;
    [SerializeField] private float maxRadius = default;
    [SerializeField] private float vertexCountToRadiusRatio = default;
    [SerializeField] private float minImpactImpulseForDestruction = default;
    [SerializeField] private float minDebrisCountToRadiusRatio = default;
    [SerializeField] private float maxDebrisCountToRadiusRatio = default;
    [SerializeField] private float minDebrisSeparationVelocity = default;
    [SerializeField] private float maxDebrisSeparationVelocity = default;

    private LineRenderer lineRenderer;
    private new Rigidbody2D rigidbody2D;
    private new PolygonCollider2D collider;

    private float radius;

    /// <summary>
    /// Initialize the asteroid with a random radius.
    /// </summary>
    public void Init()
    {
        float radius = Random.Range(minRadius, maxRadius);
        Init(radius);
    }

    /// <summary>
    /// Initialize the asteroid with the specified radius.
    /// <param name="radius">The radius of the asteroid</param>
    /// </summary>
    public void Init(float radius)
    {
        this.radius = radius;

        lineRenderer = GetComponent<LineRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider = GetComponent<PolygonCollider2D>();

        int vertexCount = Mathf.RoundToInt(vertexCountToRadiusRatio * radius);

        lineRenderer.positionCount = vertexCount;

        Vector2[] colliderPoints = new Vector2[vertexCount];

        for (int i = 0; i < vertexCount; i++)
        {
            float angle = i * 2f * Mathf.PI / vertexCount;
            float height = radius * Random.Range(minHeight, 1f);
            Vector2 position = new Vector2
            (
                height * Mathf.Cos(angle),
                height * Mathf.Sin(angle)
            );
            lineRenderer.SetPosition(i, position);
            colliderPoints[i] = position;
        }

        collider.pathCount = 1;
        collider.SetPath(0, colliderPoints);
    }

    /// <summary>
    /// Apply the specified impulse to the asteroid.
    /// </summary>
    /// <param name="impulse">The impulse to apply</param>
    public void Push(Vector2 impulse)
    {
        rigidbody2D.AddForce(impulse, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Destroy the asteroid.
    /// </summary>
    public void Vanish()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Blow up the asteroid. This will possibly create debris,
    /// meaning that if the asteroid is big enough it will spawn a few smaller asteroids in its place.
    /// The original asteroid will disappear regardless of the number of created debris.
    /// </summary>
    private void Destruct()
    {
        int minDebrisCount = Mathf.RoundToInt(minDebrisCountToRadiusRatio * radius);
        int maxDebrisCount = Mathf.RoundToInt(maxDebrisCountToRadiusRatio * radius);
        int debrisCount = Random.Range(minDebrisCount, maxDebrisCount);

        float debrisRadius = radius / Mathf.Sqrt(debrisCount);

        float debrisSeparationAngle = 2f * Mathf.PI * Random.value;
        float debrisSeparationVelocity = Random.Range(minDebrisSeparationVelocity, maxDebrisSeparationVelocity);

        for (int i = 0; i < debrisCount; i++)
        {
            debrisSeparationAngle += 2f * Mathf.PI / debrisCount;
            Vector2 debrisDirection = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * debrisSeparationAngle) * Vector2.right;

            GameObject debrisGO = Instantiate
            (
                gameObject,
                transform.position + (Vector3)(debrisRadius * debrisDirection),
                Quaternion.identity
            );
            Asteroid debris = debrisGO.GetComponent<Asteroid>();
            debris.Init(debrisRadius);

            Rigidbody2D debrisRigidbody = debrisGO.GetComponent<Rigidbody2D>();
            debrisRigidbody.velocity = rigidbody2D.velocity + debrisSeparationVelocity * debrisDirection;
            debrisRigidbody.angularVelocity = rigidbody2D.angularVelocity;
        }

        Vanish();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactImpulse = 0f;
        foreach (ContactPoint2D point in collision.contacts)
        {
            impactImpulse += point.normalImpulse;
        }

        if (impactImpulse >= minImpactImpulseForDestruction || collision.gameObject.GetComponent<Bullet>() != null)
        {
            //Push(collision.relativeVelocity)
            Destruct();
        }
    }
}
