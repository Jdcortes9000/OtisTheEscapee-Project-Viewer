using UnityEngine;
using System.Collections;

public class BaseProjectileScript : MonoBehaviour
{
    [SerializeField] private float speed;
    private int count = 0;
    
    void FixedUpdate()
    {
        count++;
        transform.Translate(transform.up * speed, Space.World);
        if (count >= 1000)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Player")
        {
            Destroy(gameObject);
            Sound sound = new Sound
            {
                Position = transform.position,
                Volume = 20f
            };
            Sound[] toBePassed = {sound};
            EventManager.SoundEvent(toBePassed);
        }
    }
}
