using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreenScript : MonoBehaviour {

    void GoToMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
