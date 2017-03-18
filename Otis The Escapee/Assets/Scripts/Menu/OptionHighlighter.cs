using UnityEngine;
using System.Collections;

public class OptionHighlighter : MonoBehaviour {

    public GameObject manager;
    void OnMouseEnter()
    {
        if(manager.GetComponent<OptionsManagerScript>().Active == true)
        {
            if (this.gameObject.tag == "Music")
            {
                manager.GetComponent<OptionsManagerScript>().index = 0;
                manager.GetComponent<OptionsManagerScript>().Animate = true;
            }
            else if (this.gameObject.tag == "Sound")
            {
                manager.GetComponent<OptionsManagerScript>().index = 1;
                manager.GetComponent<OptionsManagerScript>().Animate = true;
            }
            else if (this.gameObject.tag == "Controlls")
            {
                manager.GetComponent<OptionsManagerScript>().index = 2;
                manager.GetComponent<OptionsManagerScript>().Animate = true;
            }
            else if (this.gameObject.tag == "Screen")
            {
                manager.GetComponent<OptionsManagerScript>().index = 3;
                manager.GetComponent<OptionsManagerScript>().Animate = true;
            }
            else if (this.gameObject.tag == "Back")
            {
                manager.GetComponent<OptionsManagerScript>().index = 4;
                manager.GetComponent<OptionsManagerScript>().Animate = true;
            }
        }
        
    }
}
