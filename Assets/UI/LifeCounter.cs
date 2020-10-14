using UnityEngine;
using UnityEngine.UI;

namespace NeatSketch.AlaAsteroids
{
    [RequireComponent(typeof(Text))]
    public class LifeCounter : MonoBehaviour
    {
        [SerializeField] private char lifeIndicatorCharacter = default;

        private Text text;

        private void Awake()
        {
            text = GetComponent<Text>();
            GameManager.Instance.NumberOfLifesChanged += OnNumberOfLifesChanged;
        }

        private void OnNumberOfLifesChanged(int numberOfLifes)
        {
            text.text = new string(lifeIndicatorCharacter, numberOfLifes);
        }
    }
}