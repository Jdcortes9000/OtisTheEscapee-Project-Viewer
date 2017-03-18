using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    [SerializeField]
    string SceneName;

    public bool StopSound;
    public void LoadTheScene()
    {
        if(StopSound == true || SceneName == "WinGameScene")
        {
            for (int i = 0; i < SoundManager.instance.SFX.Length; i++)
            {
                SoundManager.instance.SFX[i].Stop();
            }
        }
        string[] ToBePassed = {"SceneChange"};
        EventManager.StringEvent(ToBePassed);
        SceneManager.LoadSceneAsync(SceneName);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
