using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab = default;
    [SerializeField] private float thrustForce = default;
    [SerializeField] private float rotationRate = default;

    private new Rigidbody2D rigidbody2D;
    private bool thrusterIsActive;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
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

        if (Input.GetKey(KeyCode.Space))
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
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}
