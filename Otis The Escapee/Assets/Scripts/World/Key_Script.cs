using UnityEngine;
using System.Collections;

public class Key_Script : MonoBehaviour {

    void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject.tag == "Player")
        {
            string[] toBePassed = {"giveKey"};
            EventManager.StringEvent(toBePassed);
            this.gameObject.SetActive(false);
        }
    }
}
