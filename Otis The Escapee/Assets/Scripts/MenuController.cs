using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour
{
    [SerializeField] private string unpauseString;
    [SerializeField] private string pauseString;
    [SerializeField] private GameObject[] pauseMenu;
    
	void Start () {
	    foreach (GameObject s in pauseMenu)
	    {
	        s.SetActive(false);
	    }
	    EventManager.PassStrings += recieveStrings;
	}

    void OnDestroy()
    {
        EventManager.PassStrings -= recieveStrings;
    }


    private void recieveStrings(string[] values)
    {
        foreach (string s in values)
        {
            if (s == unpauseString)
            {
                foreach (GameObject g in pauseMenu)
                {
                    g.SetActive(true);
                }
            }
            else if (s == pauseString)
            {
                foreach (GameObject g in pauseMenu)
                {
                    g.SetActive(false);
                }
            }
        }
    }
}
