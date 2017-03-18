using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance = null;
    public AudioSource[] SFX;
 
    void Awake ()
    {
	    if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
	}

	public void PlaySingle(AudioClip clip, int i)
    {
        SFX[i].clip = clip;
        SFX[i].Play();
    }
}
