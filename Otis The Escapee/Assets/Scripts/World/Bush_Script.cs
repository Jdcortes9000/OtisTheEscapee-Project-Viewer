using UnityEngine;
using System.Collections;

public class Bush_Script : MonoBehaviour
{
    [SerializeField] private Sprite livingBush;
    [SerializeField] private Sprite deadBush;
    [SerializeField] int NumUse = 2;
    public GameObject player;
    private int NumUseMax;

	public Color Col;
	//public AudioClip BushSound;
	public GameObject camera;
    void Start()
    {
        NumUseMax = NumUse = NumUse * 2;
        SpriteRenderer spt = this.gameObject.GetComponent<SpriteRenderer>();
        spt.sprite = livingBush;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            this.GetComponent<ParticleSystem>().Play();
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
            this.GetComponent<ParticleSystem>().Play();
            player.GetComponent<PLayerSound>().breathing = false;
            this.GetComponentInChildren<SphereCollider>().enabled = false;
            SoundManager.instance.SFX[3].Stop();
            player.GetComponent<PLayerSound>().heartbeat = false;
            SoundManager.instance.SFX[4].Stop();
            NumUse--;
            if (NumUse <= NumUseMax / 2)
            {
                SpriteRenderer spt = this.gameObject.GetComponent<SpriteRenderer>();
                spt.sprite = deadBush;
            }
            if (NumUse <= 0)
            {
                gameObject.SetActive(false);
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
