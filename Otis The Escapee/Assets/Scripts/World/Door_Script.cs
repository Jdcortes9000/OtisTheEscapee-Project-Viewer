using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Door_Script : MonoBehaviour {

    public void HitTrigger()
    {
        GetComponent<Animator>().SetTrigger("Opened");
    }
}
