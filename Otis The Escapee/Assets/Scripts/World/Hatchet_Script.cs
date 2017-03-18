using UnityEngine;
using System.Collections;

public class Hatchet_Script : MonoBehaviour {

    void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject.tag == "Player")
        {
            string[] toBePassed = {"giveHatchet"};
            EventManager.StringEvent(toBePassed);
            gameObject.SetActive(false);
        }
    }
}
