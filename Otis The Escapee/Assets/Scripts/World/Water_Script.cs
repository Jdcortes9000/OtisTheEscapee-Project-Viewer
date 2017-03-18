using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Water_Script : MonoBehaviour {
    public GameObject player;
    public AudioClip Bubbles;
    public GameObject camera;
    protected Dictionary<string, KeyCode> Keys = new Dictionary<string, KeyCode>();
    private bool pause = false;
    
    void Start()
    {
        Keys.Add("Sprint", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Sprint", "Space")));
        EventManager.PassStrings += ParseStrings;
    }

    void OnDestroy()
    {
        EventManager.PassStrings -= ParseStrings;
    }

    void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject.tag == "Player")
        {
            //SoundManager.instance.SFX[1].mute = true;
            SoundManager.instance.SFX[1].Stop();
            player.GetComponent<PLayerSound>().water = true;
        }
        //SoundManager.instance.PlaySingle2(water);
    }

    void OnTriggerStay(Collider _other)
    {
        if (_other.gameObject.tag == "Player" && !pause)
        {
            if(Input.GetKey(Keys["Sprint"]) && player.GetComponent<PlayerController>().outOfStamina == false)
            {
                string[] toBePassed = { "VPower2", "VColor0,0.407843,0.545098" };
                EventManager.StringEvent(toBePassed);
            }
            else
            {
                string[] toBePassed = { "VPower1", "VColor0,0,0" };
                EventManager.StringEvent(toBePassed);
            }
        }
    }
    void OnTriggerExit(Collider _other)
    {
        if (_other.gameObject.tag == "Player")
        {
            SoundManager.instance.SFX[1].mute = false;
        SoundManager.instance.SFX[2].Stop();
        SoundManager.instance.SFX[3].Stop();
        player.GetComponent<PLayerSound>().water = false;

        }
    }

    void ParseStrings(string[] values)
    {
        foreach (var s in values)
        {
            switch (s)
            {
                case "pause":
                {
                    pause = true;
                    break;
                }
                case "unpause":
                {
                    pause = false;
                    break;
                }
            }
        }
    }
}
