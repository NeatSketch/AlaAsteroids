using UnityEngine;

public class OutOfBoundsAsteroidCleaner : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collider)
    {
        Asteroid asteroid = collider.GetComponent<Asteroid>();
        asteroid?.Vanish();
    }
}
