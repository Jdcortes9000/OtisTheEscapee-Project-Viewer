using UnityEngine;
using System.Collections;

public class IntroScript : MonoBehaviour {

   public bool done = false;

	void Start () {
        StartCoroutine(ExecuteAfterTime(12.0f));
	}
	
	void Update () {
	    if(Input.GetKey(KeyCode.Space))
        {
            done = true;
            SoundManager.instance.SFX[0].Stop();
            this.gameObject.SetActive(false);
        }
	}

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        done = true;
        SoundManager.instance.SFX[0].Stop();
        this.gameObject.SetActive(false);
    }
}
