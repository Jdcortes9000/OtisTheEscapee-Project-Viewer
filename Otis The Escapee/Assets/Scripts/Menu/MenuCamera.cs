using UnityEngine;
using System.Collections;

public class MenuCamera : MonoBehaviour {
    public Transform CurrentPos;
    public Transform NewPos;
    public GameObject MenuScript;
    public GameObject OptionScript;
    public float journeyTime = 1.0F;
    private float startTime;
    public bool Active;
    bool MoveCamera;
    bool StartClick;
	
	void Start () {
        Active = true;
        MoveCamera = false;
        StartClick = true;
	}
	
	void Update () {
	    if(Active)
        {
            
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("AButton"))
            {
                ClickOnStart();
            }
            if(MoveCamera == true)
            {
                SwitchSection(CurrentPos, NewPos, "Menu");
            }
            
        }
        else
        {
            startTime = Time.time;
        }
        
	}

    public void SwitchSection(Transform old, Transform newpos, string location)
    {
        float fracComplete = (Time.time - startTime) / journeyTime;
        this.transform.position = Vector3.Lerp(old.position, newpos.position, fracComplete);
        if(this.transform.position == newpos.position)
        {
            Active = false;
            MoveCamera = false;
            
        }
        if (location == "Menu" && Active == false)
        {
            MenuScript.GetComponent<MenuManagerScript>().Active = true;
            MenuScript.GetComponent<MenuManagerScript>().EnableButtons();
            OptionScript.GetComponent<OptionsManagerScript>().Active = false;
            OptionScript.GetComponent<OptionsManagerScript>().DisableButtons();
        }
        else if (location == "Option" && Active == false)
        {
            MenuScript.GetComponent<MenuManagerScript>().Active = false;
            MenuScript.GetComponent<MenuManagerScript>().DisableButtons();
            OptionScript.GetComponent<OptionsManagerScript>().Active = true;
            OptionScript.GetComponent<OptionsManagerScript>().EnableButtons();
        }
    }

    public void ClickOnStart()
    { 
      if (StartClick == true)
        {
         MoveCamera = true;
         startTime = Time.time;
         StartClick = false;
        }   
    }
}
