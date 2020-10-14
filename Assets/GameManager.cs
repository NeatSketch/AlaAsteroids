using System;
using System.Collections;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private GameObject shipPrefab = default;
    [SerializeField] private int startingNumberOfLifes = default;
    [SerializeField] private float postImpactInvulnerabilityPeriod = default;
    [SerializeField] private float gameOverTimeout = default;
    [SerializeField] private AnimationCurve asteroidMassToScoreFunction = default;

    private Ship ship;
    private int numberOfLifes;
    private int score;
    private bool gameIsInProgress;

    public event Action<int> ScoreChanged;
    public event Action<int> NumberOfLifesChanged;
    public event Action GameStarted;
    public event Action GameEnded;

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        GameObject shipGO = Instantiate(shipPrefab, Vector3.zero, Quaternion.identity);
        ship = shipGO.GetComponent<Ship>();
        ship.Impacted += OnShipImpacted;

        numberOfLifes = startingNumberOfLifes;
        score = 0;
        gameIsInProgress = true;

        GameStarted?.Invoke();
        NumberOfLifesChanged?.Invoke(numberOfLifes);
        ScoreChanged?.Invoke(score);
    }

    private void StopGame()
    {
        gameIsInProgress = false;
        GameEnded?.Invoke();
    }

    private void OnShipImpacted()
    {
        numberOfLifes -= 1;
        NumberOfLifesChanged?.Invoke(numberOfLifes);
        if (numberOfLifes > 0)
        {
            StartCoroutine(RestartLife());
        }
        else
        {
            ship.Destruct();
            StopGame();
            StartCoroutine(RestartGame());
        }
    }

    private IEnumerator RestartLife()
    {
        ship.SetInvulnerable(true);

        yield return new WaitForSeconds(postImpactInvulnerabilityPeriod);

        ship.SetInvulnerable(false);
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(gameOverTimeout);

        ResetGame();
        StartGame();
    }

    private void ResetGame()
    {
        Asteroid[] asteroidsInGame = FindObjectsOfType<Asteroid>(true);
        foreach (Asteroid asteroid in asteroidsInGame)
        {
            asteroid.Vanish();
        }
    }

    public void ScoreAsteroidHit(float mass)
    {
        if (gameIsInProgress)
        {
            score += Mathf.RoundToInt(asteroidMassToScoreFunction.Evaluate(mass));
            ScoreChanged?.Invoke(score);
        }
    }
}
