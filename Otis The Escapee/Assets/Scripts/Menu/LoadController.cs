using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadController : MonoBehaviour
{
    [SerializeField] private int FileNumber;
    [SerializeField] private string[] Levels;
    [SerializeField] private int levelToLoad;
	
	void Start ()
	{
	    levelToLoad = PlayerPrefs.GetInt(string.Format("LevelFile{0}", FileNumber)) - 1;
	}

    public void LoadTheScene()
    {
        if (Levels[levelToLoad] == "Level_Test")
        {
            SoundManager.instance.SFX[0].Stop();
        }

        PlayerPrefs.SetInt("FileToLoadFrom", FileNumber);
        PlayerPrefs.Save();
        SceneManager.LoadScene(Levels[levelToLoad]);
    }
}
