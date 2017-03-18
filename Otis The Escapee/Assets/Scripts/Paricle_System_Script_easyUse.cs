using UnityEngine;
using System.Collections;

public class Paricle_System_Script_easyUse : MonoBehaviour {


	[SerializeField]
	string MessageWaitingFor=null;

	[SerializeField]
	ParticleSystem Part;

	[SerializeField]
	float MaxTime;

	private float CurrTimer;
    
	void Start () 
	{
		EventManager.PassStrings += GetStrings;
		if (MessageWaitingFor == null)
			this.gameObject.SetActive (false);
		if(Part == null)
			this.gameObject.SetActive (false);
	}
	
	void GetStrings(string[] strings)
	{
		string Mess = MessageWaitingFor;
		foreach (string s in strings)
		{
			if(s == Mess)
			if (Part != null)
				Part.Play ();
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
