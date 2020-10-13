using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float velocity = default;

    /// <summary>
    /// Initialize the bullet, making it move forward relative to its starting position and rotation.
    /// </summary>
    /// <param name="startVelocity">
    /// The starting velocity which should be equal to the world-space velocity
    /// of the weapon that shoots the bullet
    /// </param>
    public void Init(Vector2 startVelocity)
    {
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        Vector2 addedVelocity = transform.rotation * Vector2.right * velocity;
        rigidbody2D.velocity = startVelocity + addedVelocity;
    }
}
