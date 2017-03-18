using UnityEngine;
using System.Collections;

public class Shiny_Script : MonoBehaviour {

	[SerializeField]
	GameObject ShinyToPutOn;
	private bool Shinebool = false;
    
	void Start () 
	{
		if (ShinyToPutOn == null)
			this.gameObject.SetActive (false);
		else
		{
			bool check = ShinyToPutOn.GetComponent<SpriteRenderer> ().enabled;

		if(check == true)
			{
				this.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
				Shinebool = true;
			}
		else
			{
				this.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
				Shinebool = false;
			}
		}
	}
	
	void Update () 
	{
		if(ShinyToPutOn != null)
		{
			if (ShinyToPutOn.GetComponent<SpriteRenderer> ().enabled == true) 
			{
				if(Shinebool == false)
				TurnOnShine();
			}
			else
			{
				if(Shinebool == true)
				TurnOffShine();
			}
		}
	}
	void TurnOnShine()
	{
		this.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
		Shinebool = true;
	}
	void TurnOffShine ()
	{
		this.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		Shinebool = false;
	}
}
