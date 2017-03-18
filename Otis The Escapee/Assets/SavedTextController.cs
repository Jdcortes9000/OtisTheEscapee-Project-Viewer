using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SavedTextController : MonoBehaviour
{

    private float timerthingy = 0.0f;

	void Start ()
	{
	    GetComponent<Text>().enabled = false;
	}

    void Update()
    {
        timerthingy -= Time.deltaTime;
        if (timerthingy <= 0.0f)
        {
            GetComponent<Text>().enabled = false;
        }
    }

    public void TextAppear()
    {
        GetComponent<Text>().enabled = true;
        timerthingy = 2.0f;
    }
}
