using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level1_InstructioText : MonoBehaviour {

    protected Dictionary<string, KeyCode> Keys = new Dictionary<string, KeyCode>();

    void Start () {
        Keys.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W")));
        Keys.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
        Keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
        Keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
        Keys.Add("Sprint", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Sprint", "Space")));
        Keys.Add("Reusable", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Reusable", "E")));
        Keys.Add("Consumable", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Consumable", "R")));
        SetUpText();


    }

    void SetUpText()
    {
        this.transform.GetChild(0).GetComponent<TextMesh>().text = Keys["Up"].ToString() + "/" + Keys["Down"].ToString() + "/" + Keys["Left"].ToString() + "/" + Keys["Right"].ToString() + "\n to move.";
        this.transform.GetChild(1).GetComponent<TextMesh>().text = "Hold '" + Keys["Sprint"].ToString() + "' to sprint or \n to submerge on a water pond";
        this.transform.GetChild(2).GetComponent<TextMesh>().text = "The Hatchet can be used on certain trees with '" + Keys["Reusable"].ToString() + "' ";
        this.transform.GetChild(3).GetComponent<TextMesh>().text = "The pebble can be used to \n distract enemies with '" + Keys["Consumable"].ToString() + "' ";
        this.transform.GetChild(2).GetComponent<TextMesh>().text = "The Hatchet can be used on certain trees with '" + Keys["Reusable"].ToString() + "' ";
    }
}
