using UnityEngine;
using System.Collections;

public class CaveScript : MonoBehaviour {

    [SerializeField]
    private Sprite SemiUsed;
    [SerializeField]
    private Sprite Used;
    [SerializeField]
    int NumUse = 2;
    public GameObject player;
    private int NumUseMax;


	public Color Col;
	//public AudioClip CaveSound;
	public GameObject camera;
    void Start () 
	{
        NumUseMax = NumUse = NumUse * 2;
    }
    
    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            player.GetComponent<PLayerSound>().breathing = true;
            this.GetComponentInChildren<SphereCollider>().enabled = true;

        }
    }

	void OnTriggerStay(Collider _other)
	{
		if (_other.gameObject.tag == "Player")
		{
			string[] Pass = { "VPower2", "VColor" + Col.r.ToString () + "," + Col.g.ToString () + "," + Col.b.ToString () + "," + Col.a.ToString () };
			EventManager.StringEvent(Pass);
		}
	}

    void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "Player")
        {
            player.GetComponent<PLayerSound>().breathing = false;
            this.GetComponentInChildren<SphereCollider>().enabled = false;
            SoundManager.instance.SFX[3].Stop();
            player.GetComponent<PLayerSound>().heartbeat = false;
            SoundManager.instance.SFX[4].Stop();
            NumUse--;
            if (NumUse <= NumUseMax / 2)
            {
                this.GetComponent<SpriteRenderer>().sprite = SemiUsed;
            }
            if (NumUse <= 0)
            {
                this.GetComponent<SpriteRenderer>().sprite = Used;
                this.GetComponent<BoxCollider>().isTrigger = false;
            }
			RestartV ();
        }
    }

	void RestartV()
	{
		string[] toBePassed = { "VPower1", "VColor0,0,0,0" };
		EventManager.StringEvent(toBePassed);
	}
}
