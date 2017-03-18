using UnityEngine;
using System.Collections;

public class LoadScreen : MonoBehaviour
{

    [SerializeField] private GameObject LoadingScreen;

	void Start ()
    {
	    LoadingScreen.SetActive(false);
	    EventManager.PassStrings += StringParse;
	}

    void OnDestroy()
    {
        EventManager.PassStrings -= StringParse;
    }

    void StringParse(string[] values)
    {
        foreach (string s in values)
        {
            switch (s)
            {
                case "SceneChange":
                {
                    LoadingScreen.SetActive(true);
                    break;
                }
            }
        }
    }
}
