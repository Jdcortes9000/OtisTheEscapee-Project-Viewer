using System;
using UnityEngine;
using System.Collections;

public class TreeScript : MonoBehaviour
{

    [SerializeField] private Sprite LivingTree;
    [SerializeField] private Sprite DeadTree;
    [SerializeField] private EventScheduler scheduler;
    private AudioSource AudioPlayer;
    private SpriteRenderer Renderer;
    private Collider Collider;
    private Collider Collider2;
	[SerializeField]
	ParticleSystem Fallleafs;

    void Start ()
	{
	    Renderer = GetComponent<SpriteRenderer>();
        Collider = GetComponent<BoxCollider>();
        Collider2 = GetComponent<SphereCollider>();
        AudioPlayer = GetComponent<AudioSource>();
	}

    public void CutTree()
    {
        AudioPlayer.volume = SoundManager.instance.SFX[1].volume;
        GetComponent<Animator>().SetTrigger("TreeCut");
        AudioPlayer.enabled = true;
        Collider.enabled = false;
        Collider2.enabled = false;
        Sound sound = new Sound
        {
            Position = transform.position,
            Volume = 200f
        };
        Sound[] toBePassed = {sound};
        EventManager.SoundEvent(toBePassed);
		if (Fallleafs != null) 
		{
			Fallleafs.Play ();
		}
    }
}
