using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeyLabels : MonoBehaviour {

    protected Dictionary<string, KeyCode> Keys = new Dictionary<string, KeyCode>();
    public Text Consumable;
    public Text Reusable;
    public Text Map;
    void Start () {
        Keys.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W")));
        Keys.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
        Keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
        Keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
        Keys.Add("Sprint", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Sprint", "Space")));
        Keys.Add("Reusable", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Reusable", "E")));
        Keys.Add("Consumable", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Consumable", "R")));
        Keys.Add("Map", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Map", "M")));
        Consumable.text = Keys["Consumable"].ToString();
        Reusable.text = Keys["Reusable"].ToString();
        Map.text = Keys["Map"].ToString();
    }
}
