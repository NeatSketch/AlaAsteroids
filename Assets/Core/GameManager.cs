using NeatSketch.UnityUtils;
using System;
using System.Collections;
using UnityEngine;

namespace NeatSketch.AlaAsteroids
{
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

        /// <summary>
        /// Fired when the player's score is changed.
        /// </summary>
        public event Action<int> ScoreChanged;

        /// <summary>
        /// Fired when the player's number of lifes is changed.
        /// </summary>
        public event Action<int> NumberOfLifesChanged;

        /// <summary>
        /// Fired when a new game starts.
        /// </summary>
        public event Action GameStarted;

        /// <summary>
        /// Fired when the game ends.
        /// </summary>
        public event Action GameEnded;

        private void Start()
        {
            StartGame();
        }

        /// <summary>
        /// Start a new game: create a new ship, initialize the variables, & fire the necessary event handlers.
        /// </summary>
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

        /// <summary>
        /// Stop the game: destroy the ship & fire the game end event handler.
        /// </summary>
        private void StopGame()
        {
            ship.Destruct();
            gameIsInProgress = false;
            GameEnded?.Invoke();
        }

        /// <summary>
        /// Called when the ship is impacted.
        /// Decides whether to stop the game or just take a life from the player.
        /// </summary>
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
                StopGame();
                StartCoroutine(RestartGame());
            }
        }

        /// <summary>
        /// Makes the ship temporarily enter the post-impact invulnerability state.
        /// </summary>
        private IEnumerator RestartLife()
        {
            ship.SetInvulnerable(true);

            yield return new WaitForSeconds(postImpactInvulnerabilityPeriod);

            ship.SetInvulnerable(false);
        }

        /// <summary>
        /// Restarts the game after the 'Game Over' timeout.
        /// </summary>
        private IEnumerator RestartGame()
        {
            yield return new WaitForSeconds(gameOverTimeout);

            ResetGame();
            StartGame();
        }

        /// <summary>
        /// Reset the game, cleaning the mess the asteroids made.
        /// </summary>
        private void ResetGame()
        {
            Asteroid[] asteroidsInGame = FindObjectsOfType<Asteroid>(true);
            foreach (Asteroid asteroid in asteroidsInGame)
            {
                asteroid.Vanish();
            }
        }

        /// <summary>
        /// Score an asteroid hit by the player.
        /// The added score amount is decided by considering the mass of the asteroid using an animation curve.
        /// </summary>
        /// <param name="mass">The asteroid's mass</param>
        public void ScoreAsteroidHit(float mass)
        {
            if (gameIsInProgress)
            {
                score += Mathf.RoundToInt(asteroidMassToScoreFunction.Evaluate(mass));
                ScoreChanged?.Invoke(score);
            }
        }
    }
}