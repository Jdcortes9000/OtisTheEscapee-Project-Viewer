using UnityEngine;
using System.Collections;

public class DummyController : MonoBehaviour
{

    private float DummyTimer = 0.0f;
    
	void Start ()
	{
	    DummyTimer = 10.0f;
	}
	
	void Update ()
	{
	    DummyTimer -= Time.deltaTime;
	    if (DummyTimer <= 0.0f)
	    {
	        Destroy(this.gameObject);
	    }
	}
}
