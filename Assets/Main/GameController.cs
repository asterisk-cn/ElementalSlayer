using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace MyGame
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private int _defaultScore = 100;
        [SerializeField]
        private GameObject _gameOverWindow;
        [SerializeField]
        private TMP_Text _scoreText;

        public static GameController Instance;

        public int Score { get; private set; }

        void Start()
        {
            Instance = this;
            _gameOverWindow.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }

        public void GameOver()
        {
            _scoreText.text = "Score: " + Score;
            _gameOverWindow.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void AddScore(int score = 0, float multiplier = 1.0f)
        {
            if (score == 0)
            {
                score = _defaultScore;
            }
            Score += (int)(score * multiplier);
        }
    }
}
