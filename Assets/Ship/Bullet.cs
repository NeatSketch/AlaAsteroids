using UnityEngine;

namespace NeatSketch.AlaAsteroids
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float velocity = default;
        [SerializeField] private float timeToLive = default;

        /// <summary>
        /// Initialize the bullet, making it move forward relative to its starting position and rotation.
        /// The bullet will self-destroy after its 'time to live' is over.
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

            Destroy(gameObject, timeToLive);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Destroy(gameObject);
        }
    }
}