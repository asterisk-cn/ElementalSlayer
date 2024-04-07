using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class EnemyHealthBar : MonoBehaviour
    {
        private EnemyController _enemyController;
        [SerializeField]
        private Image _healthBar;

        void Start()
        {
            _enemyController = GetComponentInParent<EnemyController>();
            _healthBar.fillAmount = 1;
        }

        void Update()
        {
            _healthBar.fillAmount = (float)_enemyController.health / (float)_enemyController.maxHealth;

        }

        void LateUpdate() {
            transform.LookAt(Camera.main.transform);
        }
    }
}
