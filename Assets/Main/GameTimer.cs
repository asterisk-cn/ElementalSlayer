using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class GameTimer : MonoBehaviour
    {
        [SerializeField]
        private float _gameTime = 60.0f;
        [SerializeField]
        private Image _timerGauge;
        private float _currentTime;
        // Start is called before the first frame update
        void Start()
        {
            _currentTime = _gameTime;
            _timerGauge.fillAmount = 0;
        }

        // Update is called once per frame
        void Update()
        {
            _currentTime -= Time.deltaTime;
            _currentTime = Mathf.Clamp(_currentTime, 0, _gameTime);

            _timerGauge.fillAmount = _currentTime / _gameTime;

            if (_currentTime <= 0)
            {
                GameController.Instance.GameOver();
            }
        }
    }
}
