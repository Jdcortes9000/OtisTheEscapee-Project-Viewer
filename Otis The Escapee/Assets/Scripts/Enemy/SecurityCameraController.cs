using UnityEngine;
using System.Collections;

public class SecurityCameraController : BaseEnemyController
{
    /// <summary>
    /// The intital rotation of the camera
    /// </summary>
    private Quaternion initialRotation;
    /// <summary>
    /// The Left angle quaternion based off of the initial rotation
    /// </summary>
    private Quaternion LeftMost;
    /// <summary>
    /// The right angle quaternion based off of the initial rotation
    /// </summary>
    private Quaternion rightMost;
    /// <summary>
    /// The speed of the rotation between angles
    /// </summary>
    private float TurnTime = 0.2f;
    /// <summary>
    /// Determines the wether the camera is rotating left or right
    /// </summary>
    private bool left = true;
    /// <summary>
    /// Current angle to rotate
    /// </summary>
    [SerializeField] float angle;
    /// <summary>
    /// The alarm sound when spotted
    /// </summary>
    private AudioSource alertsound;

    [SerializeField]
    private Animator SCAnimator;

    new void Start()
    {
        base.Start();
        initialRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        LeftMost = Quaternion.Euler(0, initialRotation.eulerAngles.y - angle / 2, 0);
        rightMost = Quaternion.Euler(0, initialRotation.eulerAngles.y + angle / 2, 0);
    }
    
    /// <summary>
    /// Makes the camera slerp on an angle
    /// </summary>
    public override void LookAround()
    {
        if(left)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, LeftMost, angle * TurnTime * Time.deltaTime);
            if (Mathf.Abs(transform.rotation.eulerAngles.y - LeftMost.eulerAngles.y) < 5f)
            {
                left = !left;
            }
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rightMost, angle * TurnTime * Time.deltaTime);
            if (Mathf.Abs(transform.rotation.eulerAngles.y - rightMost.eulerAngles.y) < 5f)
            {
                left = !left;
            }
        }
        transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    /// <summary>
    /// Rotates towards detected player
    /// </summary>
    /// <param name="pos"></param>
    public override void Follow(Vector3 pos)
    {
        alertsound = GetComponent<AudioSource>();
        alertsound.volume = SoundManager.instance.SFX[1].volume;
        alertsound.enabled = true;
        RotateTo(pos);
        string[] toBePass = { string.Format("CameraSawEnemy{0},{1},{2}", pos.x, pos.y, pos.z) };
        EventManager.StringEvent(toBePass);     
        SCAnimator.SetBool("Alert", true);
    }

    /// <summary>
    /// Does lookaround function
    /// </summary>
    public override void Patrol()
    {
        LookAround();
        SCAnimator.SetBool("Alert", false);
    }

    public override void Wander()
    {
        LookAround();
        SCAnimator.SetBool("Alert", false);
    }

    /// <summary>
    /// Sets back to patrol mode
    /// </summary>
    /// <param name="pos"></param>
    public override void Investigate(Vector3 pos)
    {
        alertsound.enabled = false;
        CurrentMode = Modes.PatrolMode;
    }

    public override void SightDetection()
    {
        bool foundplayer = false;
        float currentangle = FOV / 2;
        Vector3 strt = transform.position;
        Vector3 dir = transform.rotation * Vector3.up;
        while (currentangle >= -FOV / 2)
        {
            Ray ray = new Ray(strt, Quaternion.Euler(0, currentangle, 0) * dir);
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, sightrange, 4352);
            Debug.DrawRay(strt, Quaternion.Euler(0, currentangle, 0) * dir, Color.red);

            if (hitInfo.collider != null)
            {
                if (hitInfo.transform.tag == "Player" && hitInfo.transform.gameObject.GetComponent<SpriteRenderer>().color.a >= 1f)
                {
                    player = hitInfo.collider;
                    CurrentMode = Modes.FollowMode;
                    foundplayer = true;
                    seen = true;
                    SetLightColor(1, 0, 0, 1);
                    findsound = GetComponent<AudioSource>();
                    findsound.enabled = true;
                    break;
                }
            }
            currentangle -= 1f;
        }
        if (!foundplayer && seen)
        {
            playerlastknown = player.transform.position;
            CurrentMode = Modes.InvestigateMode;
            SetLightColor(1, 1, 1, 1);
            findsound.enabled = false;
            seen = false;
        }
    }
    
    public override void RecieveSounds(Sound[] values)
    {
    }
}
