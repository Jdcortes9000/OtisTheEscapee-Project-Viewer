using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class OptionsManagerScript : MonoBehaviour {
    
    public Button[] Buttons;
    public GameObject MainCamera;
    public Transform CurrentPos;
    public Transform NewPos;
    public AudioClip Buttonsound;
    public AudioClip clicksound;
    public bool Active;
    public int index;
    public bool Animate;
    public bool Move;
    public bool keyboard;
    Color MainColor;
    bool Paxisbuffer;
    bool Naxisbuffer;
    
    void Start()
    {
        Paxisbuffer = false;
        Naxisbuffer = false;
        keyboard = false;
        Move = false;
        Animate = false;
        index = 0;
        MainColor = Buttons[0].GetComponentInChildren<Text>().color;
        Active = false;

    }
    
    void Update()
    {
        if (Active)
        {
            if (keyboard == false)
            {
                if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxisRaw("DpadVertical") == 1 || Input.GetAxisRaw("LeftJoystickVertical") == -1) && Buttons[index].enabled == true)
                {
                    if (Input.GetJoystickNames() == null || (Input.GetJoystickNames() != null && Paxisbuffer == false))
                    {
                        Animate = true;
                        if (index > 0)
                        {
                            index--;
                        }
                        else if (index == 0)
                        {
                            index = Buttons.Length - 1;
                        }
                        Paxisbuffer = true;
                    }
                }
                else if (Input.GetAxisRaw("DpadVertical") != 1 || Input.GetAxisRaw("LeftJoystickVertical") != -1)
                {
                    Paxisbuffer = false;
                }
                if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxisRaw("DpadVertical") == -1 || Input.GetAxisRaw("LeftJoystickVertical") == 1) && Buttons[index].enabled == true)
                {
                    if (Input.GetJoystickNames() == null || (Input.GetJoystickNames() != null && Naxisbuffer == false))
                    {
                        Animate = true;
                        if (index < Buttons.Length - 1)
                        {
                            index++;
                        }
                        else if (index == Buttons.Length - 1)
                        {
                            index = 0;
                        }
                        Naxisbuffer = true;
                    }
                }
                else if (Input.GetAxisRaw("DpadVertical") != -1 || Input.GetAxisRaw("LeftJoystickVertical") != 1)
                {
                    Naxisbuffer = false;
                }
            }
                UpdateButton(index);
                if (Move == true)
                {
                    MainCamera.GetComponent<MenuCamera>().Active = true;
                    MainCamera.GetComponent<MenuCamera>().SwitchSection(CurrentPos, NewPos, "Menu");
                    if (MainCamera.GetComponent<MenuCamera>().Active == false)
                        Move = false;
                }

                if (index == 0) //Menu Volume
                {
                    if (Animate == true)
                    {
                        SoundManager.instance.PlaySingle(Buttonsound, 1);
                        Buttons[index].GetComponent<Animator>().Play("NewButton");
                        Animate = false;
                    }
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("AButton") && keyboard == false && Buttons[index].enabled == true)
                    {
                        SoundManager.instance.PlaySingle(clicksound, 1);
                        DisableStuff();
                        Buttons[index].GetComponent<MusicVolumeScript>().EnableIt();
                    }
                }
                else if (index == 1) //Sound Volume
                {
                    if (Animate == true)
                    {
                    SoundManager.instance.PlaySingle(Buttonsound, 1);
                    Buttons[index].GetComponent<Animator>().Play("ContinueButton");
                        Animate = false;
                    }
                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("AButton") && keyboard == false && Buttons[index].enabled == true)
                    {
                        SoundManager.instance.PlaySingle(clicksound, 1);
                        DisableStuff();
                        Buttons[index].GetComponent<GameVolumeScript>().EnableIt();
                    }
                }
                else if (index == 2) //Controller
                {
                    if (Animate == true)
                    {
                    SoundManager.instance.PlaySingle(Buttonsound, 1);
                    Buttons[index].GetComponent<Animator>().Play("InstructionButton");
                        Animate = false;
                    }
                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("AButton") && keyboard == false && Buttons[index].enabled == true)
                    {
                    if(keyboard == false)
                    {
                        SoundManager.instance.PlaySingle(clicksound, 1);
                        DisableStuff();
                        Buttons[index].GetComponent<KeyBindingScript>().EnableIt();
                    }
                    
                    }
                }
                else if (index == 3) //Screen size
                {
                    if (Animate == true)
                    {
                    SoundManager.instance.PlaySingle(Buttonsound, 1);
                    Buttons[index].GetComponent<Animator>().Play("OptionButton");
                        Animate = false;
                    }
                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("AButton") && keyboard == false && Buttons[index].enabled == true)
                    {
                        SoundManager.instance.PlaySingle(clicksound, 1);
                        DisableStuff();
                        Buttons[index].GetComponent<ScreenSizeScript>().EnableIt();
                    }
                }
                else if (index == 4) //Exit
                {
                    if (Animate == true)
                    {
                        SoundManager.instance.PlaySingle(Buttonsound, 1);
                        Buttons[index].GetComponent<Animator>().Play("CreditsButton");
                        Animate = false;
                    }
                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("AButton") && keyboard == false && Buttons[index].enabled == true)
                    {
                        SoundManager.instance.PlaySingle(clicksound, 1);
                        DisableStuff();
                        Buttons[index].GetComponent<BackToMenuScript>().MoveToMenu();
                        Active = false;
                    }
                }
                        
        }

    }

    void UpdateButton(int idx)
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].GetComponentInChildren<Text>().color = MainColor;
        }
        Buttons[idx].GetComponentInChildren<Text>().color = Color.yellow;
    }

    public void MoveToMenu()
    {
        DisableStuff();
        Move = true;
        index = 0;
    }

    public void PlayClick()
    {
        SoundManager.instance.PlaySingle(clicksound, 1);
    }

    public void DisableStuff()
    {
        Buttons[3].GetComponent<ScreenSizeScript>().DisableIt();
        Buttons[0].GetComponent<MusicVolumeScript>().DisableIt();
        Buttons[1].GetComponent<GameVolumeScript>().DisableIt();
        Buttons[2].GetComponent<KeyBindingScript>().DisableIt();
    }

    public void DisableButtons()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].enabled = false;
        }
    }

    public void EnableButtons()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].enabled = true;
        }
    }

    public void EnableActive()
    {
        Active = true;
    }

    public void DisableActive()
    {
        Active = false;
    }
}
