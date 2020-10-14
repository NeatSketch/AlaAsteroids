using UnityEngine;

namespace NeatSketch.AlaAsteroids
{
    public class OutOfBoundsAsteroidCleaner : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D collider)
        {
            Asteroid asteroid = collider.GetComponent<Asteroid>();
            asteroid?.Vanish();
        }
    }
}