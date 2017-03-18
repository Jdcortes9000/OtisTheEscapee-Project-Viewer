using UnityEngine;
using System.Collections;

public class Sticks_Script : MonoBehaviour {

    [SerializeField]
    private Sprite Sticks;
    [SerializeField]
    private Sprite Broken;
    private AudioSource StepOnStick;
    private SpriteRenderer Renderer;
    int getinstick;

    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        StepOnStick = GetComponent<AudioSource>();
        Renderer.sprite = Sticks;
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Player")
        {
            StepOnStick.volume = SoundManager.instance.SFX[1].volume;
            Renderer.sprite = Broken;
            StepOnStick.enabled = true;
            Sound sound = new Sound
            {
                Position = transform.position,
                Volume = 100f
            };
            Sound[] toBePassed = { sound };
            EventManager.SoundEvent(toBePassed);
        }
    }

    void OnTriggerExit(Collider obj)
    {
        if (obj.tag == "Player")
        {
            StepOnStick.enabled = false;
        }
    }
}
