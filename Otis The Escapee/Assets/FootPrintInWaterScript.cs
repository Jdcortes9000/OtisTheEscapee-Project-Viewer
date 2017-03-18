using UnityEngine;
using System.Collections;

public class FootPrintInWaterScript : MonoBehaviour
{

    public bool CANBESEEN = true;

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Water")
        {
            CANBESEEN = false;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "Water")
        {
            CANBESEEN = true;
        }
    }
}
