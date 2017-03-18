using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CreditsManager : MonoBehaviour {

    public Button ExitCredits;
    
	void Update () {
	if(Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("BButton"))
        {
            ExitCredits.GetComponent<SceneLoader>().LoadTheScene();
        }
	}
}
