using UnityEngine;
using System.Collections;

public class Paricle_Script : MonoBehaviour {

	[SerializeField]
	ParticleSystem Part;

	[SerializeField]
	float MaxTime;

	private float CurrTimer;

	void Start()
	{
		EventManager.PassStrings += GetStrings;
		CurrTimer = 0;
	}

	void GetStrings(string[] strings)
	{
		foreach (string s in strings)
		{
			switch (s)
			{
			case "StartBleed":
				if (Part != null)
					Part.Play ();
				break;
			}
		}
	}

	void Update()
	{
		if (Part != null)
		if (Part.isPlaying == true) 
		{
			if (CurrTimer >= MaxTime) 
			{
				CurrTimer = 0f;
				Part.Stop ();
			} 
			else
				CurrTimer += Time.deltaTime;
		}
	}
}
