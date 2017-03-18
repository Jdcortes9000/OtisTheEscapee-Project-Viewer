using UnityEngine;
using System.Collections;

public class ThrowItem : MonoBehaviour {

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Wall"|| coll.tag == "Tree")
        {
            Destroy(this.gameObject);
            Sound sound = new Sound
            {
                Position = transform.position,
                Volume = 20f
            };
            Sound[] toBePassed = { sound };
            EventManager.SoundEvent(toBePassed);
        }
    }
}
