using UnityEngine;
using System.Collections;

public class MapItemController : MonoBehaviour
{
    [SerializeField] public int Ident;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            string[] toBePassed = { string.Format("Entered{0}", Ident) };
            EventManager.StringEvent(toBePassed);
        }
    }
}
