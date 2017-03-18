using UnityEngine;
using System.Collections;

public class HealthKit : MonoBehaviour {

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            this.gameObject.active = false;
        }
    }
}
