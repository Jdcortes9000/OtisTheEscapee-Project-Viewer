using UnityEngine;
using System.Collections;

public class FoundPlayerSScript : MonoBehaviour {
	private float Timer=0;
	void GetRid()
	{
		Destroy (this.gameObject);
	}

	void Update()
	{
		if (Timer >= .5f) 
		{
			GetRid ();
		} 
		else
			Timer += Time.deltaTime;
	}

	public void WhereIs(Vector3 EnemyPos)
	{
		this.transform.position = EnemyPos;
	}

}
