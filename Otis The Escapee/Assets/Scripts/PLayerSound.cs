using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class PLayerSound : MonoBehaviour {

    public GameObject player;
    public AudioClip background;
    public AudioClip grass;
    public AudioClip Water;
    public AudioClip Run;
    public AudioClip bubbles;
    public AudioClip breath;
    public AudioClip Heart;
    public AudioClip jailbreak;
    public GameObject intro;
    public bool water;
    public bool breathing;
    public bool heartbeat;
    bool walking;
    bool panting = false;
    Dictionary<string, KeyCode> Keys = new Dictionary<string, KeyCode>();

    void Start () {
        SoundManager.instance.SFX[0].volume = PlayerPrefs.GetFloat("GameMusic", 1);
        SoundManager.instance.SFX[1].volume = PlayerPrefs.GetFloat("GameSound", 1);
        Keys.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W")));
        Keys.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
        Keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
        Keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
        Keys.Add("Sprint", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Sprint", "Space")));
        Keys.Add("Reusable", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Reusable", "E")));
        Keys.Add("Consumable", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Consumable", "R")));
        if (intro != null)
        {
            player.GetComponent<PlayerController>().enabled = false;
            SoundManager.instance.PlaySingle(jailbreak, 0);
        }
        else if( intro == null)
        {
            SoundManager.instance.PlaySingle(background, 0);
        }
        for (int i = 1; i < SoundManager.instance.SFX.Length;i ++)
        {
            SoundManager.instance.SFX[i].volume = SoundManager.instance.SFX[1].volume;
            SoundManager.instance.SFX[i].enabled = true;
            SoundManager.instance.SFX[i].mute = false;
            SoundManager.instance.SFX[i].clip = null;
        }
        water = false;
        breathing = false;
        heartbeat = false;
	}
	
    void WalkingSounds()
    {
        if(water == true)
        {
            if (SoundManager.instance.SFX[2].isPlaying == false)
            {
                SoundManager.instance.PlaySingle(Water,2);
            }
        }
        else if (Input.GetKey(Keys["Sprint"]))
        {
            if(player.GetComponent<PlayerController>().outOfStamina == true)
            {
                if( panting == false)
                {
                    panting = true;
                    SoundManager.instance.PlaySingle(grass, 1);
                    SoundManager.instance.PlaySingle(breath, 3);
                    StartCoroutine(StopPanting(5.0f));
                }
            }
           else if (SoundManager.instance.SFX[1].isPlaying == false || walking == true)
            {
                SoundManager.instance.PlaySingle(Run,1);
                walking = false;
            }
        }
        else
        {
            if (SoundManager.instance.SFX[1].isPlaying == false || walking == false)
            {
                SoundManager.instance.PlaySingle(grass,1);
                walking = true;
            }
        }
        
    }

	void FixedUpdate ()
    {
        if(intro != null)
        {
            if (intro.GetComponent<IntroScript>().done == true)
            {
                if (SoundManager.instance.SFX[0].isPlaying == false)
                {
                    player.GetComponent<PlayerController>().enabled = true;
                    SoundManager.instance.PlaySingle(background, 0);
                }
                CheckForMovement();
                CheckForWater();
                CheckForBush();
                CheckForEnemies();
            }
        }
        else if(intro == null)
        {
            CheckForMovement();
            CheckForWater();
            CheckForBush();
            CheckForEnemies();
        }
         
    }

    void CheckForMovement()
    {
        if (Input.GetKey(Keys["Up"]))
        {

            WalkingSounds();
        }
        else if (Input.GetKey(Keys["Down"]))
        {
            WalkingSounds();
        }
        else if (Input.GetKey(Keys["Left"]))
        {
            WalkingSounds();
        }
        else if (Input.GetKey(Keys["Right"]))
        {
            WalkingSounds();
        }

        else
        {
            if(walking == true || walking == false)
            {
                SoundManager.instance.SFX[1].Stop();
            }

            if(water == true)
            {
                SoundManager.instance.SFX[2].Stop();
            }
            
        }
    }

    void CheckForWater()
    {
        if (Input.GetKey(Keys["Sprint"]) && water == true && player.GetComponent<PlayerController>().outOfStamina == false)
        {
            player.transform.GetChild(3).GetComponent<ParticleSystem>().Play();
            if (SoundManager.instance.SFX[3].isPlaying == false)
            {
                SoundManager.instance.PlaySingle(bubbles, 3);
            }
        }
        else if(water == true)
        {
            SoundManager.instance.SFX[3].Stop();
            player.transform.GetChild(3).GetComponent<ParticleSystem>().Stop();
        }
        else
        {
            player.transform.GetChild(3).GetComponent<ParticleSystem>().Stop();
        }
    }

    void CheckForBush()
    {
        if(SoundManager.instance.SFX[3].isPlaying == false && breathing == true)
        {
            SoundManager.instance.PlaySingle(breath,3);
        }
        
    }

    void CheckForEnemies()
    {
        if(SoundManager.instance.SFX[4].isPlaying == false && heartbeat == true)
        {
            SoundManager.instance.PlaySingle(Heart,4);
        }
    }

    IEnumerator StopPanting(float time)
    {
        yield return new WaitForSeconds(time);
        panting = false;
        SoundManager.instance.SFX[3].Stop();
    }
}
