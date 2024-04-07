using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyGame
{
    public class SceneChanger : MonoBehaviour
    {
        public string sceneName;

        public void SceneChange()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
