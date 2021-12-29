using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Overtail.Arcade.Street
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private CarSpawner[] spawners;
        [SerializeField] private bool CanRetry = true;

        private void Awake()
        {
            player = GameObject.FindObjectOfType<Player>();
            if (player == null) throw new ArgumentNullException("No Player in scene");

            spawners = GameObject.FindObjectsOfType<CarSpawner>();

            player.GotRunOver += OnCrash;
            player.ReachedGoal += () => StartCoroutine(GameOver());
        }

        private void Start()
        {
            Reset();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) Reset();
        }

        private void OnCrash(Car car)
        {
            SpriteRenderer sp;
            if (car.TryGetComponent<SpriteRenderer>(out sp))
            {
                sp.color = Color.magenta;
            }

            if (CanRetry)
            {
                StartCoroutine(WaitForRetry());
            }
            else
            {
                StartCoroutine(GameOver());
            }
        }

        private IEnumerator WaitForRetry()
        {
            PauseGame();
            yield return StartCoroutine(WaitForKey(KeyCode.Space));
            Reset();
        }

        private void Reset()
        {
            foreach (Car c in GameObject.FindObjectsOfType<Car>())
            {
                Destroy(c.gameObject);
            }

            foreach (CarSpawner s in spawners)
            {
                s.Reset();
            }

            player.Reset();

            ResumeGame();

            StartCoroutine(FastForward(15f));
        }

        private IEnumerator FastForward(float seconds)
        {
            Debug.Log("zoom zoom " + seconds + "seconds");
            Time.timeScale = 10;
            yield return new WaitForSeconds(seconds);
            Time.timeScale = 1;
            Debug.Log("Back2Normal");
        }

        private IEnumerator GameOver()
        {
            // Show GAMEOVER text
            PauseGame();
            Debug.Log("YOU WON");
            yield return StartCoroutine(WaitForKey(KeyCode.Space));
        }

        private IEnumerator WaitForKey(KeyCode code)
        {
            while (!Input.GetKeyDown(code))
            {
                yield return null;
            }
        }


        private void PauseGame()
        {
            // PAUSED
            Time.timeScale = 0;
        }

        private void ResumeGame()
        {
            // Remove "PAUSED" text
            Time.timeScale = 1;
        }
    }
}