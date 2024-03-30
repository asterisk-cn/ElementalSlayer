using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class EnemyManager : MonoBehaviour
    {
        public List<EnemyController> enemies = new List<EnemyController>();
        void Start()
        {
            foreach (EnemyController enemy in FindObjectsOfType<EnemyController>())
            {
                enemies.Add(enemy);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
