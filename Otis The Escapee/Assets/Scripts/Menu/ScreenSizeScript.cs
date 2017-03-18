using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ScreenSizeScript : MonoBehaviour {

    public Toggle Windowed;
    public Toggle FullScreen;
    public GameObject manager;
    public AudioClip sound;
    public AudioClip clicksound;
    public int index;
    public bool Active;
    Color MainColor;
    bool soundOn;
    int width;
    int height;
    void Start () {
        width = Screen.width;
        height = Screen.height;
        soundOn = false;
        DisableIt();
        index = 0;
        Active = false;
        MainColor = Windowed.GetComponentInChildren<Text>().color;
        if(Screen.fullScreen == true)
        {
            index = 1;
            Windowed.isOn = false;
            FullScreen.isOn = true;
        }
        else if(Screen.fullScreen == false)
        {
            index = 0;
            Windowed.isOn = true;
            FullScreen.isOn = false;
        }
	}
	
	void Update () {
	    if(manager.GetComponent<OptionsManagerScript>().keyboard == true)
        {
            soundOn = true;
            ActivateKeyboard();
        }
	}
    public void ActivateKeyboard()
    {
        if(Active == true)
        {
           
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("BButton"))
            {
                SoundManager.instance.PlaySingle(clicksound, 1);
                manager.GetComponent<OptionsManagerScript>().keyboard = false;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxisRaw("DpadVertical") == 1 || Input.GetAxisRaw("LeftJoystickVertical") == -1)
            {
                SoundManager.instance.PlaySingle(sound, 1);
                index = 0;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxisRaw("DpadVertical") == -1 || Input.GetAxisRaw("LeftJoystickVertical") == 1)
            {
                SoundManager.instance.PlaySingle(sound, 1);
                index = 1;
            }

            if (index == 0)
            {
                Windowed.GetComponentInChildren<Text>().color = Color.yellow;
                FullScreen.GetComponentInChildren<Text>().color = MainColor;
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("AButton") && Windowed.interactable == true)
                {
                    SoundManager.instance.PlaySingle(clicksound, 1);
                    ChangeToWindowed(true);
                }
            }
            else if (index == 1)
            {
                Windowed.GetComponentInChildren<Text>().color = MainColor;
                FullScreen.GetComponentInChildren<Text>().color = Color.yellow;
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("AButton") && FullScreen.interactable == true)
                {
                    SoundManager.instance.PlaySingle(clicksound, 1);
                    ChangeToFull(true);
                }
            }
            if(manager.GetComponent<OptionsManagerScript>().keyboard == false)
            {
                Windowed.GetComponentInChildren<Text>().color = MainColor;
                FullScreen.GetComponentInChildren<Text>().color = MainColor;
            }
        }
        
    }

    public void PlayClick()
    {
        if(soundOn == true)
        SoundManager.instance.PlaySingle(clicksound, 1);
    }

    public void ChangeToWindowed(bool active)
    {
        if(active == true)
        {
            FullScreen.isOn = false;
            Windowed.isOn = true;
            Windowed.interactable = false;
            FullScreen.interactable = true;
            Screen.SetResolution(800,600,false);
        }
        else
        {
            Windowed.interactable = true;
        }

    }

    public void ChangeToFull(bool active)
    {
        if (active == true)
        {
            Windowed.isOn = false;
            FullScreen.isOn = true;
            FullScreen.interactable = false;
            Windowed.interactable = true;
            Screen.SetResolution(width, height, true);
        }
        else
        {
            FullScreen.interactable = true;
        }
    }

    public void DisableIt()
    {
        manager.GetComponent<OptionsManagerScript>().keyboard = false;
        Active = false;
        Windowed.enabled = false;
        Windowed.transform.position = new Vector3(Windowed.transform.position.x, Windowed.transform.position.y, -11);
        FullScreen.enabled = false;
        FullScreen.transform.position = new Vector3(FullScreen.transform.position.x, FullScreen.transform.position.y, -11);
    }

    public void EnableIt()
    {
        manager.GetComponent<OptionsManagerScript>().keyboard = true;
        Active = true;
        Windowed.enabled = true;
        Windowed.transform.position = new Vector3(Windowed.transform.position.x, Windowed.transform.position.y, 1);
        FullScreen.enabled = true;
        FullScreen.transform.position = new Vector3(FullScreen.transform.position.x, FullScreen.transform.position.y, 1);
    }
}
