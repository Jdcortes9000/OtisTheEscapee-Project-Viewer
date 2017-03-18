using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
public class MusicVolumeScript : MonoBehaviour {

    public GameObject portion;
    public Slider Menu;
    public Slider Game;
    public GameObject manager;
    public AudioClip clicksound;
    public AudioClip preview;
    int i = 0;
    float menucurr;
    float gamecurr;
    bool active;
	
	void Start () {
        SoundManager.instance.PlaySingle(preview, 2);
        SoundManager.instance.SFX[2].mute = true;
        Menu.value = PlayerPrefs.GetFloat("MenuMusic", 1);
        Game.value = PlayerPrefs.GetFloat("GameMusic", 1);
        Menu.enabled = false;
        Game.enabled = false;
        portion.transform.position = new Vector3(portion.transform.position.x, portion.transform.position.y, -11);
       
        active = false;
        menucurr = Menu.value;
        gamecurr = Game.value;
    }
	
	void Update () {
        SoundManager.instance.SFX[0].volume = Menu.value;
        SoundManager.instance.SFX[2].volume = Game.value;
        if(menucurr != Menu.value)
        {
            i = 0;
            menucurr = Menu.value;
        }
        else if(gamecurr != Game.value)
        {
            i = 1;
            gamecurr = Game.value;
        }
        if(manager.GetComponent<OptionsManagerScript>().keyboard == true)
        {
            ActivateKeyboard();
        }
    }

    public void ActivateKeyboard()
    {
        if (active == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("BButton"))
            {
                SoundManager.instance.PlaySingle(clicksound, 1);
                manager.GetComponent<OptionsManagerScript>().keyboard = false;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxisRaw("DpadVertical") == -1 || Input.GetAxisRaw("LeftJoystickVertical") == 1)
            {
                i = 1;
                
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxisRaw("DpadVertical") == 1 || Input.GetAxisRaw("LeftJoystickVertical") == -1)
            {
                i = 0;
                
            }
            if (i == 0)
            {
                SoundManager.instance.SFX[0].mute = false;
                SoundManager.instance.SFX[2].mute = true;
                Menu.transform.GetChild(4).GetComponent<Text>().color = Color.yellow;
                Game.transform.GetChild(4).GetComponent<Text>().color = Color.white;
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("LeftJoystickHorizontal") > 0 || Input.GetAxis("DpadHorizontal") > 0)
                {
                    if (Menu.value < 1)
                        Menu.value += 0.01f;
                }
                else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("LeftJoystickHorizontal") < 0 || Input.GetAxis("DpadHorizontal") < 0)
                {
                    if (Menu.value > 0)
                        Menu.value -= 0.01f;
                }
            }
           else if (i == 1)
            {
                SoundManager.instance.SFX[0].mute = true;
                SoundManager.instance.SFX[2].mute = false;
                Menu.transform.GetChild(4).GetComponent<Text>().color = Color.white;
                Game.transform.GetChild(4).GetComponent<Text>().color = Color.yellow;
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("LeftJoystickHorizontal") > 0 || Input.GetAxis("DpadHorizontal") > 0)
                {
                    if (Game.value < 1)
                        Game.value += 0.01f;
                }
                else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("LeftJoystickHorizontal") < 0 || Input.GetAxis("DpadHorizontal") < 0)
                {
                    if (Game.value > 0)
                        Game.value -= 0.01f;
                }
            }
            if(manager.GetComponent<OptionsManagerScript>().keyboard == false)
            {
                Menu.transform.GetChild(4).GetComponent<Text>().color = Color.white;
                Game.transform.GetChild(4).GetComponent<Text>().color = Color.white;
                SoundManager.instance.SFX[0].mute = false;
                SoundManager.instance.SFX[2].mute = true;
            }
        }
        
    }

    public void DisableIt()
    {
        SoundManager.instance.SFX[0].mute = false;
        SoundManager.instance.SFX[2].mute = true;
        manager.GetComponent<OptionsManagerScript>().keyboard = false;
        active = false;
        Menu.enabled = false;
        Game.enabled = false;
        portion.transform.position = new Vector3(portion.transform.position.x, portion.transform.position.y, -11);
        PlayerPrefs.SetFloat("MenuMusic", Menu.value);
        PlayerPrefs.SetFloat("GameMusic", Game.value);
    }

    public void EnableIt()
    {
        active = true;
        Menu.enabled = true;
        Game.enabled = true;
        portion.transform.position = new Vector3(portion.transform.position.x, portion.transform.position.y, 0);
        manager.GetComponent<OptionsManagerScript>().keyboard = true;
    }
}
