using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class LevelSelectManager : MonoBehaviour {
    public Button Lv1;
    public Button Lv2;
    public Button Lv3;
    public Button exc;
    public int index;
    public AudioClip music;
    public AudioClip sound;
    public AudioClip clicksound;
    public bool Animate;
    bool Paxisbuffer;
    bool Naxisbuffer;
    Color MainColor;
    
    void Start () {
        Paxisbuffer = false;
        Naxisbuffer = false;
        SoundManager.instance.SFX[0].volume = PlayerPrefs.GetFloat("MenuMusic", 1);
        if (SoundManager.instance.SFX[0].isPlaying == false)
        {
            SoundManager.instance.PlaySingle(music, 0);
        }
        SoundManager.instance.SFX[1].volume = PlayerPrefs.GetFloat("MenuSound", 1);
        index = 0;
        MainColor = Lv1.GetComponentInChildren<Text>().color;
        Animate = false;
    }
	
	void Update () {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("BButton"))
        {
            exc.GetComponent<SceneLoader>().LoadTheScene();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("LeftJoystickHorizontal") == -1 || Input.GetAxisRaw("DpadHorizontal") == -1)
        {
            if (Input.GetJoystickNames() == null || (Input.GetJoystickNames() != null && Naxisbuffer == false))
            {
                Animate = true;
                if (index > 0)
                {
                    index--;
                }
                else if (index == 0)
                {
                    index = 2;
                }
                Naxisbuffer = true;
            }             
        }
        else if(Input.GetAxis("LeftJoystickHorizontal") != -1 || Input.GetAxisRaw("DpadHorizontal") != -1)
        {
            Naxisbuffer = false;
        }
         if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("LeftJoystickHorizontal") == 1 || Input.GetAxisRaw("DpadHorizontal") == 1)
        {
            if (Input.GetJoystickNames() == null || (Input.GetJoystickNames() != null && Paxisbuffer == false))
            {
                Animate = true;
                if (index < 2)
                {
                    index++;
                }
                else if (index == 2)
                {
                    index = 0;
                }
                Paxisbuffer = true;
            }          
        }
         else if(Input.GetAxis("LeftJoystickHorizontal") != 1 || Input.GetAxisRaw("DpadHorizontal") != 1)
        {
            Paxisbuffer = false;
        }

        if (index == 0)
        {
            if(Animate == true)
            {
                SoundManager.instance.PlaySingle(sound, 1);
                Animate = false;
            }
            Lv1.GetComponentInChildren<Text>().color = Color.yellow;
            Lv2.GetComponentInChildren<Text>().color = MainColor;
            Lv3.GetComponentInChildren<Text>().color = MainColor;
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("AButton"))
            {
                SoundManager.instance.PlaySingle(clicksound, 1);
                Lv1.GetComponent<SceneLoader>().LoadTheScene();
            }
        }
        else if (index == 1)
        {
            if (Animate == true)
            {
                SoundManager.instance.PlaySingle(sound, 1);
                Animate = false;
            }
            Lv1.GetComponentInChildren<Text>().color = MainColor;
            Lv2.GetComponentInChildren<Text>().color = Color.yellow;
            Lv3.GetComponentInChildren<Text>().color = MainColor;
           
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("AButton"))
            {
                SoundManager.instance.PlaySingle(clicksound, 1);
                Lv2.GetComponent<SceneLoader>().LoadTheScene();
            }
        }
        else if (index == 2)
        {
            if (Animate == true)
            {
                SoundManager.instance.PlaySingle(sound, 1);
                Animate = false;
            }
            Lv1.GetComponentInChildren<Text>().color = MainColor;
           Lv2.GetComponentInChildren<Text>().color = MainColor;
            Lv3.GetComponentInChildren<Text>().color = Color.yellow;
            
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("AButton"))
            {
                SoundManager.instance.PlaySingle(clicksound, 1);
                Lv3.GetComponent<SceneLoader>().LoadTheScene();
            }
        }

    }
    public void PlayClick()
    {
        SoundManager.instance.PlaySingle(clicksound, 1);
    }
}
