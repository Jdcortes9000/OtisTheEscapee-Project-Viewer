using UnityEngine;
using System.Collections;

public class Lightcontroller : MonoBehaviour {
    
    private float lightingtime;
    public Light lr;
    private float savelight;
    private bool iflight;

    void Start () {
        lightingtime = 10f;
        lr = GetComponent<Light>();
        savelight = lr.intensity;
        iflight = false;
    }

	void Fliplight()
    {
        if (lr.intensity == savelight)
        {
            lightingtime -= Time.deltaTime;
            lr.intensity = savelight * 10;
        }
        else
            lr.intensity = savelight;
    }

	void Update () {
        lightingtime -= Time.deltaTime;
        if (lightingtime <= 0)
        {
            AudioSource source = GetComponent<AudioSource>();
            source.volume = SoundManager.instance.SFX[1].volume;
            if (source.enabled == true)
            {
                source.enabled = false;
                iflight = false;
                lightingtime = Random.Range(5f, 10f);
            }
            else
            {
                source.enabled = true;
                iflight = true;
                lightingtime = Random.Range(1f, 2f);
            }
            lr.intensity = savelight * 10;          
        }
        if (iflight)
            Fliplight();
        else
            lr.intensity = savelight;
    }
}
