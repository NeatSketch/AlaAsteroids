using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace NeatSketch.AlaAsteroids
{
    [RequireComponent(typeof(Text))]
    public class ScoreCounter : MonoBehaviour
    {
        private Text text;

        private void Awake()
        {
            text = GetComponent<Text>();
            GameManager.Instance.ScoreChanged += OnScoreChanged;
        }

        private void OnScoreChanged(int score)
        {
            text.text = score.ToString(CultureInfo.CurrentUICulture);
        }
    }
}