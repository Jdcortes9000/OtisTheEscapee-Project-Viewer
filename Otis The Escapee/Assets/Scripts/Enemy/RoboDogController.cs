using UnityEngine;
using System.Collections;

public class RoboDogController : BaseEnemyController
{
    [SerializeField] private Animator DogAnimator;

    private float basespeedsave;
    public override void Alert()
    {
        basespeedsave = agent.speed;          
        Damage_amount = Damage_amount * 0.05f;
        isAlert = false;
    }

    public override void Follow(Vector3 pos)
    {
        if (!player.name.StartsWith("Dummy") && !player.name.Contains("Foot"))
        {
            string[] awareNessString = { string.Format("Aware{0}", 0.3f) };
            EventManager.StringEvent(awareNessString);
        }
		DogAnimator.SetBool("Attack", false);
        if (isAlert)
        {
            DogAnimator.SetBool("Alert", true);
        }
        var aSources = GetComponents<AudioSource>();
        findsound = aSources[0];
        robotwalk = aSources[1];
        if (Mathf.Sqrt(Mathf.Pow(transform.position.x - pos.x, 2) + Mathf.Pow(transform.position.z - pos.z, 2)) <= sightrange / 2)
            robotwalk.enabled = true;
        if (Mathf.Sqrt(Mathf.Pow(transform.position.x - pos.x, 2) + Mathf.Pow(transform.position.z - pos.z, 2)) > sightrange / 2)
            robotwalk.enabled = false;
        RotateTo(pos);
        MoveTo(pos);
        if (alerted)
        {
            if (player != null && player.name.Contains("Player"))
            {
                if (Timer_HitPlayerPerSce <= 0f)
                {
                    agent.speed = agent.speed * 1.5f;
                    Timer_HitPlayerPerSce = Max_Time_For_Hits;
                }
                if (Mathf.Sqrt(Mathf.Pow(transform.position.x - pos.x, 2) + Mathf.Pow(transform.position.z - pos.z, 2)) <= sightrange / 5)
                {
                    robotwalk.enabled = true;
                    agent.speed = basespeedsave;
                    Damage_Player();
                }
                else if (Mathf.Sqrt(Mathf.Pow(transform.position.x - pos.x, 2) + Mathf.Pow(transform.position.z - pos.z, 2)) > sightrange / 5)
                {
                    Timer_HitPlayerPerSce -= Time.deltaTime;
                    robotwalk.enabled = false;
                }
            }
        }
    }

    /// <summary>
    /// Detects when an Object is inside this Object,
    /// This is were you would want to handle direct Object collisions
    /// </summary>
    /// <param name="obj">The Object that collided into this Object</param>
    void OnTriggerStay(Collider obj)
    {
        if (Paused)
        {
            return;
        }
        ignorefootpirints = true;
        if (obj.tag == "Player")
        {
            player = obj;
            CurrentMode = Modes.FollowMode;
            if (!alerted && Timer_HitPlayerPerSce <= 0f)
            {
                Damage_Player();
                Timer_HitPlayerPerSce = Max_Time_For_Hits;
            }
             Timer_HitPlayerPerSce -= Time.deltaTime;
        }
        if (obj.tag == "Enemy")
            wpos = RandomNavSphere(transform.position, 5, -1);
    }
}
