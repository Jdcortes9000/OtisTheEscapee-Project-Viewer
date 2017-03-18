using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InstructionsScene : MonoBehaviour {
    
    public GameObject instruction;
    public Transform CurrentPos;
    public Transform NewPos;
    public GameObject MainCamera;
    bool Active;

    void Start()
    {
        instruction.transform.position = new Vector3(instruction.transform.position.x, instruction.transform.position.y, -11);
        Active = false;
    }

    void Update()
    {
        if (Active)
        {
            MainCamera.GetComponent<MenuCamera>().SwitchSection(CurrentPos, NewPos, "Option");
            if (MainCamera.GetComponent<MenuCamera>().Active == false)
                Active = false;
        }

    }
    public void MoveToOptions()
    {
        MainCamera.GetComponent<MenuCamera>().Active = true;
        Active = true;
    }
    public void ShowInstructions()
    {
        instruction.transform.position = new Vector3(instruction.transform.position.x, instruction.transform.position.y, 1);
    }
    public void HideInstructions()
    {
        instruction.transform.position = new Vector3(instruction.transform.position.x, instruction.transform.position.y, -11);
    }
}
