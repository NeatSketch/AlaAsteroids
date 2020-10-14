using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen = default;

    private void Awake()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.GameStarted += OnGameStarted;
        gameManager.GameEnded += OnGameEnded;
    }

    private void OnGameStarted()
    {
        gameOverScreen.SetActive(false);
    }

    private void OnGameEnded()
    {
        gameOverScreen.SetActive(true);
    }
}
