using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndSceneScript : MonoBehaviour {

    public Light thelight;
    public AudioClip walk;
    public AudioClip lights;
    public GameObject player;
    public Camera cam;
    public AudioClip what;
    public AudioClip fu;
    public GameObject end;
    int backs = 0;
    bool skipable = false;

	void Start () {
        SoundManager.instance.PlaySingle(walk, 1);
        StartCoroutine(Skip(0.5f));
        StartCoroutine(StopWalking(6.0f));
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && skipable == true)
        {
            for(int i = 0; i < SoundManager.instance.SFX.Length;i++)
            {
                SoundManager.instance.SFX[i].Stop();
            }
            SceneManager.LoadScene("CreditsScene");
        }

    }

    IEnumerator Skip(float time)
    {
        yield return new WaitForSeconds(time);
        skipable = true;
    }

    IEnumerator StopWalking(float time)
    {
        yield return new WaitForSeconds(time);
        SoundManager.instance.SFX[1].Stop();
        player.transform.GetChild(0).parent = null;
        SoundManager.instance.PlaySingle(lights, 2);
        StartCoroutine(WHAT(0.5f));
        SoundManager.instance.SFX[2].loop = false;
        thelight.enabled = true;
        StartCoroutine(BackTheCamera(2.0f));
    }

    IEnumerator BackTheCamera(float time)
    {
        yield return new WaitForSeconds(time);
        if (backs < 5)
        {
            SoundManager.instance.PlaySingle(lights, 2);
            cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + 15, cam.transform.position.z);
            backs++;
            StartCoroutine(BackTheCamera(1.0f));
        }
        else
        {
            SoundManager.instance.PlaySingle(lights, 2);
            cam.transform.position = new Vector3(cam.transform.position.x, 2, cam.transform.position.z);
            StartCoroutine(FU(2.0f));
        }
        
    }
    
    IEnumerator WHAT(float time)
    {
        yield return new WaitForSeconds(time);
        SoundManager.instance.PlaySingle(what, 1);
        SoundManager.instance.SFX[1].loop = false;
    }

    IEnumerator FU(float time)
    {
        yield return new WaitForSeconds(time);
        SoundManager.instance.PlaySingle(fu, 1);
        StartCoroutine(END(0.5f));
    }

    IEnumerator END(float time)
    {
        yield return new WaitForSeconds(time);
        SoundManager.instance.PlaySingle(lights, 2);
        end.SetActive(true);
        StartCoroutine(credits(5.0f));
    }

    IEnumerator credits(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("CreditsScene");
    }
}
