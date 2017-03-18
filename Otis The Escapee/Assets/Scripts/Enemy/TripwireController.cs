using UnityEngine;
using System.Collections;

public class TripwireController : MonoBehaviour {

    private AudioSource gothrough;

    void Start () {
        gothrough = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Player")
        {
            gothrough = GetComponent<AudioSource>();
            gothrough.enabled = true;
            string[] toBePass = { string.Format("PlayerGT{0},{1},{2}", obj.transform.position.x, obj.transform.position.y, obj.transform.position.z) };
            EventManager.StringEvent(toBePass);
            Destroy(gameObject);
        }
    }
}
