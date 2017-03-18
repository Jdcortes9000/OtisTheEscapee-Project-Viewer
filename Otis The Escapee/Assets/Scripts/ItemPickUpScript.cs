using UnityEngine;
using System.Collections;

public class ItemPickUpScript : MonoBehaviour {

    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
