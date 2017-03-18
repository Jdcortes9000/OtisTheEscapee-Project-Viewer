using UnityEngine;
using System.Collections;

public class BackToMenuScript : MonoBehaviour {

    public Transform CurrentPos;
    public Transform NewPos;
    public GameObject MainCamera;
    bool Active;
    
    void Start () {
        Active = false;
    }
	
	void Update () {
        if (Active)
        {
            MainCamera.GetComponent<MenuCamera>().SwitchSection(CurrentPos, NewPos, "Menu");
            if (MainCamera.GetComponent<MenuCamera>().Active == false)
                Active = false;
        }
    }
    public void MoveToMenu()
    {
        MainCamera.GetComponent<MenuCamera>().Active = true;
        Active = true;
    }
}
