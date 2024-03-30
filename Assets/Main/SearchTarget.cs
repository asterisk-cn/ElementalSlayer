using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SearchTarget : MonoBehaviour
    {
        private EnemyController _enemyController;
        void Start()
        {
            _enemyController = GetComponentInParent<EnemyController>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                _enemyController.Target = other.gameObject;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                _enemyController.Target = null;
            }
        }
    }
}
