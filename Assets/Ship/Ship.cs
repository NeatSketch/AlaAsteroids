using System;
using UnityEngine;

namespace NeatSketch.AlaAsteroids
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class Ship : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab = default;
        [SerializeField] private float thrustForce = default;
        [SerializeField] private float rotationRate = default;
        [SerializeField] private int impactLayer = default;

        /// <summary>
        /// Fired when the ship is impacted by another object.
        /// </summary>
        public event Action Impacted;

        private new Rigidbody2D rigidbody2D;
        private bool thrusterIsActive;
        private bool invulnerable;

        private Animator animator;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            thrusterIsActive = Input.GetKey(KeyCode.UpArrow);

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(0f, 0f, rotationRate * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(0f, 0f, -rotationRate * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shoot();
            }
        }

        private void FixedUpdate()
        {
            if (thrusterIsActive)
            {
                rigidbody2D.AddForce(thrustForce * transform.right, ForceMode2D.Force);
            }
        }

        private void Shoot()
        {
            GameObject bulletGO = Instantiate(bulletPrefab, transform.position, transform.rotation);
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            bullet.Init(rigidbody2D.velocity);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == impactLayer && !invulnerable)
            {
                Impacted?.Invoke();
            }
        }

        public void SetInvulnerable(bool invulnerable)
        {
            this.invulnerable = invulnerable;
            animator.SetBool("Invulnerable", invulnerable);
        }

        public void Destruct()
        {
            gameObject.SetActive(false);

            Destroy(gameObject);
        }
    }
}