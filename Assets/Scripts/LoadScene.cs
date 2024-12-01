using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace AugStone.Helper
{
    public class LoadScene : MonoBehaviour
    {
        
        public void JumpScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);  
        }

        public void QuitApplication()
        {
            Application.Quit();
        }

    }

}