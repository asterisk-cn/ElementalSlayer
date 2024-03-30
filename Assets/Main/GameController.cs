using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyGame
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _gameOverWindow;

        public static GameController Instance;

        void Start()
        {
            Instance = this;
            _gameOverWindow.SetActive(false);
        }

        public void GameOver()
        {
            _gameOverWindow.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
