using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapController : MonoBehaviour
{
    protected Dictionary<string, KeyCode> Keys = new Dictionary<string, KeyCode>();
    private bool MapActive = false;
    [SerializeField] private GameObject[] MapComponents;

    void Start()
    {
        Keys.Add("Map", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Map", "M")));
        EventManager.PassStrings += RecieveStrings;
    }

    void Update()
    {
        if (Input.GetKeyDown(Keys["Map"]))
        {
            MapActive = !MapActive;
            if (MapActive)
            {
                for (int i = 0; i < MapComponents.Length; i++)
                {
                    MapComponents[i].SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < MapComponents.Length; i++)
                {
                    MapComponents[i].SetActive(false);
                }
            }
        }
    }

    public void RecieveStrings(string[] values)
    {
        foreach (string s in values)
        {
            if (s.StartsWith("Entered"))
            {
                string temp = s.Remove(0, 7);
                for (int i = 0; i < MapComponents.Length; i++)
                {
                    if (MapComponents[i] != null)
                    {
                        if (!MapComponents[i].GetComponent<Image>().enabled && MapComponents[i].GetComponent<MapItemController>().Ident == int.Parse(temp))
                        {
                            MapComponents[i].GetComponent<Image>().enabled = true;
                        }
                    }
                }
            }
        }
    }

    void OnDestory()
    {
        EventManager.PassStrings -= RecieveStrings;
    }
}
