using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class KeyBindingScript : MonoBehaviour {

    public GameObject portion;
    public Button[] keys;
    public GameObject manager;
    public AudioClip sound;
    public AudioClip clicksound;
    private Dictionary<string, KeyCode> KeyDictionary = new Dictionary<string, KeyCode>();
    private GameObject currentkey;
    bool active;
    int i = -1;
    bool Paxisbuffer;
    bool Naxisbuffer;
    bool Isrepeated = false;
    void Start () {
        Paxisbuffer = false;
        Naxisbuffer = false;
        active = false;
        KeyDictionary.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Up","W")));
        KeyDictionary.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
        KeyDictionary.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
        KeyDictionary.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
        KeyDictionary.Add("Sprint", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Sprint", "Space")));
        KeyDictionary.Add("Reusable", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Reusable", "E")));
        KeyDictionary.Add("Consumable", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Consumable", "R")));
        KeyDictionary.Add("Map", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Map", "M")));
        keys[0].transform.GetChild(0).GetComponent<Text>().text = KeyDictionary["Up"].ToString();
        keys[1].transform.GetChild(0).GetComponent<Text>().text = KeyDictionary["Down"].ToString();
        keys[2].transform.GetChild(0).GetComponent<Text>().text = KeyDictionary["Left"].ToString();
        keys[3].transform.GetChild(0).GetComponent<Text>().text = KeyDictionary["Right"].ToString();
        keys[4].transform.GetChild(0).GetComponent<Text>().text = KeyDictionary["Sprint"].ToString();
        keys[5].transform.GetChild(0).GetComponent<Text>().text = KeyDictionary["Reusable"].ToString();
        keys[6].transform.GetChild(0).GetComponent<Text>().text = KeyDictionary["Consumable"].ToString();
        keys[7].transform.GetChild(0).GetComponent<Text>().text = KeyDictionary["Map"].ToString();
        DisableIt();
	}

	void Update () {
        if (manager.GetComponent<OptionsManagerScript>().keyboard == true)
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
                manager.GetComponent<OptionsManagerScript>().keyboard = false;
                SoundManager.instance.PlaySingle(clicksound, 1);
                active = false;
                i = -1;
            }
            if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxisRaw("DpadVertical") == -1 || Input.GetAxisRaw("LeftJoystickVertical") == 1)
            {
                if (Input.GetJoystickNames() == null || (Input.GetJoystickNames() != null && Naxisbuffer == false))
                {
                    SoundManager.instance.PlaySingle(sound, 1);
                    if (i < keys.Length - 1)
                        i++;
                    else
                    {
                        i = 0;
                    }
                    Naxisbuffer = true;
                }
                    
            }
            else if (Input.GetAxisRaw("DpadVertical") != -1 || Input.GetAxisRaw("LeftJoystickVertical") != 1)
            {
                Naxisbuffer = false;
            }
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxisRaw("DpadVertical") == 1 || Input.GetAxisRaw("LeftJoystickVertical") == -1))
            {
                if (Input.GetJoystickNames() == null || (Input.GetJoystickNames() != null && Paxisbuffer == false))
                {
                    SoundManager.instance.PlaySingle(sound, 1);
                    if (i > 0)
                        i--;
                    else
                    {
                        i = keys.Length - 1;
                    }
                    Paxisbuffer = true;
                }                                   
            }
            else if (Input.GetAxisRaw("DpadVertical") != 1 || Input.GetAxisRaw("LeftJoystickVertical") != -1)
            {
                Paxisbuffer = false;
            }
            for (int j = 0; j < keys.Length; j++)
            {
                keys[j].transform.GetChild(0).GetComponent<Text>().color = Color.white;
            }
            if(i > -1)
            keys[i].transform.GetChild(0).GetComponent<Text>().color = Color.yellow;
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("AButton"))
            {
                SoundManager.instance.PlaySingle(clicksound, 1);
                StartCoroutine(ExecuteAfterTime(0.3f));
            }
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        if(i >-1 && active == true)
            ChangeKey(keys[i].gameObject);
    }

    void OnGUI()
    {
        
        if(currentkey != null)
        {
            Event trigger = Event.current;
            if(trigger.isKey)
            {
                foreach(var key in KeyDictionary)
                {
                    if(key.Value == trigger.keyCode)
                    {
                        Isrepeated = true;
                        break;
                    }
                    Isrepeated = false;
                }
                if(Isrepeated == false)
                {
                    KeyDictionary[currentkey.name] = trigger.keyCode;
                    currentkey.transform.GetChild(0).GetComponent<Text>().text = trigger.keyCode.ToString();
                    currentkey = null;
                    SaveKeys();
                }
                else
                {
                    KeyDictionary[currentkey.name] = KeyDictionary[currentkey.name];
                    currentkey.transform.GetChild(0).GetComponent<Text>().text = currentkey.transform.GetChild(0).GetComponent<Text>().text;
                    currentkey = null;
                    SaveKeys();
                }               
            }
        }
    }

    public void PlayClick()
    {
        SoundManager.instance.PlaySingle(clicksound, 1);
    }

    public void ChangeKey(GameObject clicked)
    {
        currentkey = clicked;
    }

    public void SaveKeys()
    {
        foreach (var key in KeyDictionary)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }
        PlayerPrefs.Save();
    }

    public void DisableIt()
    {
        //i = -1;
        manager.GetComponent<OptionsManagerScript>().keyboard = false;
        active = false;
        for (int j = 0; j < keys.Length; j++)
        {
            keys[j].enabled = false;

        }
        portion.transform.position = new Vector3(portion.transform.position.x, portion.transform.position.y, -11);

    }

    public void EnableIt()
    {
        i = -1;
        manager.GetComponent<OptionsManagerScript>().keyboard = true;
        active = true;
        for (int j = 0; j < keys.Length; j++)
        {
            keys[j].enabled = true;
        }
        portion.transform.position = new Vector3(portion.transform.position.x, portion.transform.position.y, 0);
    }
}
