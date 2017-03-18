using UnityEngine;
using System.Collections;

public class ButtonHighlightScene : MonoBehaviour {

    public GameObject manager;
	void OnMouseEnter()
    {
        if(manager.GetComponent<MenuManagerScript>().Active == true)
        {
            if (this.gameObject.tag == "New")
            {
                manager.GetComponent<MenuManagerScript>().index = 0;
                manager.GetComponent<MenuManagerScript>().Animate = true;
            }
            else if (this.gameObject.tag == "Continue")
            {
                manager.GetComponent<MenuManagerScript>().index = 1;
                manager.GetComponent<MenuManagerScript>().Animate = true;
            }
            else if (this.gameObject.tag == "LevelSelect")
            {
                manager.GetComponent<MenuManagerScript>().index = 2;
                manager.GetComponent<MenuManagerScript>().Animate = true;
            }
            else if (this.gameObject.tag == "Intructions")
            {
                manager.GetComponent<MenuManagerScript>().index = 3;
                manager.GetComponent<MenuManagerScript>().Animate = true;
            }
            else if (this.gameObject.tag == "Options")
            {
                manager.GetComponent<MenuManagerScript>().index = 4;
                manager.GetComponent<MenuManagerScript>().Animate = true;
            }
            else if (this.gameObject.tag == "Credits")
            {
                manager.GetComponent<MenuManagerScript>().index = 5;
                manager.GetComponent<MenuManagerScript>().Animate = true;
            }
            else if (this.gameObject.tag == "Exit")
            {
                manager.GetComponent<MenuManagerScript>().index = 6;
                manager.GetComponent<MenuManagerScript>().Animate = true;
            }
        }
    }
}
