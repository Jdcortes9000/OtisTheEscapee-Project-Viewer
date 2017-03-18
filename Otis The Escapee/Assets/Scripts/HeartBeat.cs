using UnityEngine;
using System.Collections;

public class HeartBeat : MonoBehaviour {

    public GameObject player;
    AudioSource init;
    float distance = 0.0f;

    void Start () {
        init = SoundManager.instance.SFX[4];
	}

    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Enemy")
        {
            player.GetComponent<PLayerSound>().heartbeat = true;
        }
    }

    void OnTriggerStay(Collider coll)
    {
        distance = Mathf.Sqrt(Mathf.Pow(coll.transform.position.x - player.transform.position.x,2) + Mathf.Pow(coll.transform.position.y - player.transform.position.y,2));
        distance = (distance % 10)/10;
        SoundManager.instance.SFX[4].volume = 1 - distance;
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "Enemy")
        {
            player.GetComponent<PLayerSound>().heartbeat = false;
            SoundManager.instance.SFX[4].Stop();
        }
    }
}
