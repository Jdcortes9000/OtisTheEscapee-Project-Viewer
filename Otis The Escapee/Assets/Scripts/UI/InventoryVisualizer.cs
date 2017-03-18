using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum VizualizationType { Key, SingleUse, MultiUse}

public class InventoryVisualizer : MonoBehaviour
{
    [SerializeField] private Image visual;
    [SerializeField] private VizualizationType type;
    [SerializeField] private bool hasObject;
    [SerializeField] private Sprite[] PossibleSprites;
    private string EventManagerHandle;
    
	void Start()
	{
	    EventManager.PassStrings += ParseStrings;
	    switch (type)
	    {
	        case VizualizationType.Key:
	            EventManagerHandle = "KEY";
	            break;
	        case VizualizationType.SingleUse:
                EventManagerHandle = "SUO";
                break;
	        case VizualizationType.MultiUse:
                EventManagerHandle = "MUO";
                break;
	        default:
	            throw new ArgumentOutOfRangeException();
	    }
	}

    void OnDestroy()
    {
        EventManager.PassStrings -= ParseStrings;
    }

    void ParseStrings(string[] values)
    {
        foreach (string s in values)
        {
            if (s.StartsWith(EventManagerHandle))
            {
                hasObject = true;
                string temp = s.Remove(0, 3);
                int index;
                if (!int.TryParse(temp, out index))
                {
                    Debug.Log("Could not parse int from " + temp);
                }
                else
                {
                    if (index >= PossibleSprites.Length)
                    {
                        index = PossibleSprites.Length - 1;
                    }
                    else if (index < 0)
                    {
                        hasObject = false;
                    }

                    if (hasObject)
                    {
                        visual.sprite = PossibleSprites[index];
                        visual.color = Color.white;
                    }
                    else
                    {
                        visual.color = Color.clear;
                    }
                }
            }
        }
    }
}
