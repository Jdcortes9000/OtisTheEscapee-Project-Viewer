using UnityEngine;
using System.Collections;

public class SoundAnimation : MonoBehaviour
{

    [SerializeField] private GameObject SoundAnimator;
    
	void Start ()
	{
	    EventManager.PassSounds += getSounds;
	}
	
	void Update () {
	
	}

    void OnDestroy()
    {
        EventManager.PassSounds -= getSounds;
    }

    void getSounds(Sound[] values)
    {
        foreach (Sound s in values)
        {
            GameObject thing = (UnityEngine.GameObject)Instantiate(SoundAnimator, s.Position, Quaternion.identity);
            thing.GetComponent<SoundAnimationControler>().strength = s.Volume / 10;
        }
    }
}
