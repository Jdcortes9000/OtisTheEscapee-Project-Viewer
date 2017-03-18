using UnityEngine;
using System.Collections;

public class Level1_instructionCollider : MonoBehaviour {
    
	void Start () {
        this.GetComponent<TextMesh>().color = Color.clear;
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            this.GetComponent<TextMesh>().color = Color.white;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            this.GetComponent<TextMesh>().color = Color.clear;
    }
}
