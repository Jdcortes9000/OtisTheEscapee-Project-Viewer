using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManagerScript : MonoBehaviour
{

    public Button[] Buttons;
    public AudioClip music;
    public AudioClip Buttonsound;
    public AudioClip clicksound;
    public bool Active;
    public int index;
    public bool Animate;
    Color MainColor;
    bool Paxisbuffer;
    bool Naxisbuffer;
    
    void Start ()
    {
        Paxisbuffer = false;
        Naxisbuffer = false;
        if(SoundManager.instance.SFX[0].isPlaying == false || SoundManager.instance.SFX[0].clip != music)
        SoundManager.instance.PlaySingle(music, 0);
        Animate = false;
        index = 0;
        MainColor = Buttons[0].GetComponentInChildren<Text>().color;
        Active = false;
	}

	void Update ()
    {
        if(Active)
        {
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxisRaw("DpadVertical") == 1 || Input.GetAxisRaw("LeftJoystickVertical") == -1) &&  Buttons[index].enabled == true)
            {
                if(Input.GetJoystickNames() == null || (Input.GetJoystickNames() != null && Paxisbuffer == false))
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
            else if(Input.GetAxisRaw("DpadVertical") != 1 || Input.GetAxisRaw("LeftJoystickVertical") != -1)
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
            else if(Input.GetAxisRaw("DpadVertical") != -1 || Input.GetAxisRaw("LeftJoystickVertical") != 1)
            {
                Naxisbuffer = false;
            }
            UpdateButton(index);
            
            
            if (index == 0)
            {
                if(Animate == true)
                {
                    SoundManager.instance.PlaySingle(Buttonsound, 1);
                    Buttons[index].GetComponent<Animator>().Play("NewButton");
                    Animate = false;
                }
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("AButton") && Buttons[index].enabled == true)
                {
                    SoundManager.instance.PlaySingle(clicksound, 1);
                    Buttons[index].GetComponent<SceneLoader>().LoadTheScene();
                }
            }
            else if (index == 1)
            {
                if (Animate == true)
                {
                    SoundManager.instance.PlaySingle(Buttonsound, 1);
                    Buttons[index].GetComponent<Animator>().Play("ContinueButton");
                    Animate = false;
                }
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("AButton") && Buttons[index].enabled == true)
                {
                    SoundManager.instance.PlaySingle(clicksound, 1);
                    Buttons[index].GetComponent<SceneLoader>().LoadTheScene();
                }
            }
            else if (index == 2)
            {
                if (Animate == true)
                {
                    SoundManager.instance.PlaySingle(Buttonsound, 1);
                    Buttons[index].GetComponent<Animator>().Play("ContinueButton");
                    Animate = false;
                }
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("AButton") && Buttons[index].enabled == true)
                {
                    SoundManager.instance.PlaySingle(clicksound, 1);
                    Buttons[index].GetComponent<SceneLoader>().LoadTheScene();
                }
            }
            else if (index == 3)
            {
                if (Animate == true)
                {
                    SoundManager.instance.PlaySingle(Buttonsound, 1);
                    Buttons[index].GetComponent<Animator>().Play("InstructionButton");
                    Animate = false;
                }
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("AButton") && Buttons[index].enabled == true)
                {
                    SoundManager.instance.PlaySingle(clicksound, 1);
                    Buttons[index].GetComponent<InstructionsScene>().ShowInstructions();
                }
            }
            else if (index == 4)
            {
                if (Animate == true)
                {
                    SoundManager.instance.PlaySingle(Buttonsound, 1);
                    Buttons[index].GetComponent<Animator>().Play("OptionButton");
                    Animate = false;
                }
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("AButton") && Buttons[index].enabled == true)
                {
                    SoundManager.instance.PlaySingle(clicksound, 1);
                    Buttons[index].GetComponent<InstructionsScene>().HideInstructions();
                    Buttons[index].GetComponent<InstructionsScene>().MoveToOptions();
                    Active = false;
                }
            }
            else if (index == 5)
            {
                if (Animate == true)
                {
                    SoundManager.instance.PlaySingle(Buttonsound, 1);
                    Buttons[index].GetComponent<Animator>().Play("CreditsButton");
                    Animate = false;
                }
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("AButton") && Buttons[index].enabled == true)
                {
                    SoundManager.instance.PlaySingle(clicksound, 1);
                    Buttons[index].GetComponent<SceneLoader>().LoadTheScene();
                }
            }
            else if( index == 6)
            {
                if (Animate == true)
                {
                    SoundManager.instance.PlaySingle(Buttonsound, 1);
                    Buttons[index].GetComponent<Animator>().Play("ExitButton");
                    Animate = false;
                }
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("AButton") && Buttons[index].enabled == true)
                {
                    SoundManager.instance.PlaySingle(clicksound, 1);
                    Buttons[index].GetComponent<SceneLoader>().ExitGame();
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

    public void PlayClick()
    {
        SoundManager.instance.PlaySingle(clicksound, 1);
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
}
