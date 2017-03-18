using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FootPrints : MonoBehaviour {

    public GameObject prefab;
    public GameObject player;
    public GameObject[] footprints;
    public float speed;
    public Sprite left;
    public Sprite right;
    private float sprintSpeed;
    private Dictionary<string, KeyCode> KeyDictionary = new Dictionary<string, KeyCode>();
    bool reset = true;

	void Start () {
        sprintSpeed = speed / 2;
        KeyDictionary.Add("Sprint", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Sprint", "Space")));
        for (int i = 0; i < footprints.Length; i++)
        {      
            if(i%2 == 0)
            {
                footprints[i].GetComponent<SpriteRenderer>().sprite = right;
            }
            else
            {
                footprints[i].GetComponent<SpriteRenderer>().sprite = left;
            }
        }
	}
	
	void Update () {
	    if(reset == true)
        {
            reset = false;
            if(Input.GetKey(KeyDictionary["Sprint"]))
            {
                for (int i = 0; i < footprints.Length; i++)
                {
                    StartCoroutine(ExecuteAfterTime(i * sprintSpeed + sprintSpeed, i));
                }
            }
            else
            {
                for (int i = 0; i < footprints.Length; i++)
                {
                    StartCoroutine(ExecuteAfterTime(i * speed + speed, i));
                }
            }
            
        }
       
	}

    IEnumerator ExecuteAfterTime(float time, int idx)
    {
        yield return new WaitForSeconds(time);
        ChangePosition(idx);
        if(idx == footprints.Length - 1)
        {
            reset = true;
        }
    }

    void ChangePosition(int i)
    {
        float y = player.transform.eulerAngles.y;
        footprints[i].transform.rotation = Quaternion.Euler(90, player.transform.rotation.eulerAngles.y, player.transform.rotation.eulerAngles.z);
        if (i%2 == 0)
        {
            if(y % 180 > 45 && y % 180 < 136)
                footprints[i].transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.2f, player.transform.position.z + 0.2f);
            else
            footprints[i].transform.position = new Vector3(player.transform.position.x - 0.5f, player.transform.position.y, player.transform.position.z);
            if (y % 360 > 315 || y % 360 < 135)
                footprints[i].GetComponent<SpriteRenderer>().sprite = left;
            else
                footprints[i].GetComponent<SpriteRenderer>().sprite = right;
        }
        else
        {
            if (y % 180 > 45 && y % 180 < 136)
                footprints[i].transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.2f, player.transform.position.z -0.5f);
            else
                footprints[i].transform.position = new Vector3(player.transform.position.x + 0.5f, player.transform.position.y, player.transform.position.z);
            if (y % 360 > 315 || y % 360 < 135)
                footprints[i].GetComponent<SpriteRenderer>().sprite = right;
            else
                footprints[i].GetComponent<SpriteRenderer>().sprite = left;
        }
    }
}
