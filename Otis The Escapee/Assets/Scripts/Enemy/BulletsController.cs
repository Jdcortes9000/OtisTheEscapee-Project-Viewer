using UnityEngine;
using System.Collections;

public class BulletsController : MonoBehaviour
{
    /// <summary>
    /// bullets existing time
    /// </summary>
    float timer;
    /// <summary>
    /// player's pos
    /// </summary>
    public Vector3 playerpos;

    public float damage;

    void Start()
    {
        timer = 3f;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        Checkifhit(playerpos);
        if (timer <= 0)
        {
            Destroy(gameObject);
            timer = 3f;
        }
        else
            MoveToPlayer(playerpos);
    }

    /// <summary>
    /// let bullet go to player
    /// </summary>
    /// <param name="pos"></param>
    public void MoveToPlayer(Vector3 pos)
    {
        transform.position = Vector3.MoveTowards(transform.position, pos, 5f*Time.deltaTime);
    }
    /// <summary>
    /// check if the bullet hit player or not
    /// </summary>
    /// <param name="obj"></param>
    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Player")
        { 
            Damage_Player();
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// if bullet hit player give player damage
    /// </summary>
    void Damage_Player()
    {
        string dam = damage.ToString();
        string topass = "Damage" + dam;
        string[] toBePassed = { string.Format(topass) };
        EventManager.StringEvent(toBePassed);
    }
    /// <summary>
    /// if not hit player, disappear
    /// </summary>
    /// <param name="pos"></param>
    void Checkifhit(Vector3 pos)
    {
        if (transform.position == pos)
            Destroy(gameObject);
    }
}
