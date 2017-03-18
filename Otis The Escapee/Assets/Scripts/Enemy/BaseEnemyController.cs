using System;
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Random = UnityEngine.Random;
public class BaseEnemyController : MonoBehaviour {


    #region Variables - All varaibles that make up the base enemy
    /// <summary>
    /// how many point in path
    /// </summary>
    [SerializeField]
    GameObject[] path;
    /// <summary>
    /// Different states for the AI
    /// </summary>
    public enum Modes
    {
        PatrolMode = 0,
        FollowMode = 1,
        WanderingMode = 2,
        InvestigateMode = 3,
        LookaroundMode = 4
    };
    /// <summary>
    /// random dierction when look around
    /// </summary>
    private int random;
    /// <summary>
    /// count point in patroling mode
    /// </summary>
    protected int count;
    /// <summary>
    /// The current state of the AI
    /// </summary>
    public Modes CurrentMode;
    /// <summary>
    /// Determines wether the AI is on full alert or not
    /// </summary>
    public bool isAlert;
    /// <summary>
    /// Determines whether a player was seen during the last frame
    /// </summary>
    protected bool seen;
    /// <summary>
    /// The current direction the Object is facing
    /// </summary>
    protected float heading;
    /// <summary>
    /// The default speed of all enemys
    /// </summary>
    private float basespeed = 2f;
    /// <summary>
    /// The current velocity of the Object
    /// </summary>
    public float speed;
    /// <summary>
    /// Value indicating the base AI sight range
    /// </summary>
    private float basesightrange = 10f;
    /// <summary>
    /// Value indicating how far the Object can see
    /// </summary>
    protected float sightrange;
    /// <summary>
    /// Value indicating the base AI FOV range
    /// </summary>
    private float baseFOV = 70f;
    /// <summary>
    /// Value indicating how wide the Objects view range is
    /// </summary>
    protected float FOV;
    /// <summary>
    /// The current player that is being tracked (May be empty so always check)
    /// </summary>
    protected Collider player;
    /// <summary>
    /// The vector position of the players last known position
    /// </summary>
    protected Vector3 playerlastknown;
    /// <summary>
    /// The collider of this object
    /// </summary>
    protected Collider coll;
    /// <summary>
    /// The time count for lookaround
    /// </summary>
    protected float time;
    /// <summary>
    /// The time count for investigate
    /// </summary>
    public float InvestigateTime;
    /// <summary>
    /// Determines if the player has seen this enemy or not
    /// </summary>
    private bool PlayerHasSeen = false;
    /// <summary>
    /// play once when find player
    /// </summary>
    protected AudioSource findsound;
    /// <summary>
    /// play ramdomly
    /// </summary>
    protected AudioSource robotwalk;
    /// <summary>
    /// count robotwalk play time
    /// </summary>
    protected float robottime;
    /// <summary>
    /// check if robotwalk play
    /// </summary>
    protected bool robotplay;
    /// <summary>
    /// The initial location of the spotlight attached to this object
    /// </summary>
    private Vector3 InitialSpotlightLoc;
    /// <summary>
    /// I have no clue
    /// </summary>
    bool Adjust = false;
    /// <summary>
    /// The Object that will be dropped as the ghost of the player
    /// </summary>
	[SerializeField] GameObject GhostPlayerObj;
	/// <summary>
	/// How long that Enemy has to wait to hit player again
	/// </summary>
	protected float Timer_HitPlayerPerSce;
	/// <summary>
	/// The max time for hit.
	/// </summary>
	protected float Max_Time_For_Hits = 1f;
	/// <summary>
	/// The damage amount, How much damage the enemy can do in one hit
	/// </summary>
	[SerializeField]
	public float Damage_amount;
	[SerializeField]
	private GameObject FoundplayerS;
    /// <summary>
    /// This objects NaveMeshAgent component for navmesh navigation
    /// </summary>
    protected NavMeshAgent agent;
    /// <summary>
    /// Determines whether the object is going back to patrolling or not
    /// </summary>
    private bool IsGoingBack;
    /// <summary>
    /// Determines whether the object has gotten back to it's patrol area
    /// </summary>
    private bool ReachedPatrol;
    
    public bool IsMenu = false;

    protected bool Paused = false;
    public Vector3 wpos;
    protected bool alerted;
	private float TimerForFPS =.5f;
    private float wandertime;
	[SerializeField] private Animator ManAnimator=null;
	bool Footprint =false;
    /// <summary>
    /// Determines whether something has priority over footprints or not
    /// </summary>
    protected bool ignorefootpirints = false;
    #endregion

    #region ModeFunctions - All functions for each diffrent mode
    /// <summary>
    /// Moves the Object between predetermined points on a path
    /// </summary>
    public virtual void Patrol()
    {
        ignorefootpirints = false;
        if (agent.velocity == Vector3.zero)
        {
            count++;
            if (count > path.Length - 1)
            {
                count = 0;
            }
            agent.destination = path[count].transform.position;
            if (ReachedPatrol)
            {
                IsGoingBack = false;
            }
            ReachedPatrol = true;
        }
        if (IsGoingBack)
        {
            RotateTo(transform.position + agent.velocity * speed);
        }
        else
            RotateTo(path[count].transform.position);
    }

    /// <summary>
    /// Moves the Object to follow the passed in position
    /// </summary>
    /// <param name="pos">The position vector to move to</param>
    public virtual void Follow(Vector3 pos)
    {
		//Aware
        if (!player.name.StartsWith("Dummy") && !player.name.Contains("Foot"))
        {
            string[] awareNessString = { string.Format("Aware{0}", 0.3f) };
            EventManager.StringEvent(awareNessString);
        }
        var aSources = GetComponents<AudioSource>();
        findsound = aSources[0];
        robotwalk = aSources[1];
        robottime -= Time.deltaTime;
        if (robottime <= 0)
        {
            robottime = Random.Range(3f, 5f);
            robotplay = !robotplay;
        }
        if (robotplay)
        {
            robotwalk.enabled = true;
        }
        else
        {
            robotwalk.enabled = false;
        }  
        RotateTo(pos);
        MoveTo(pos);
    }

    /// <summary>
    /// Moves the Object to walk around the map in random directions
    /// </summary>
    public virtual void Wander()
    {
        ignorefootpirints = false;
        wandertime -= Time.deltaTime;
        RotateTo(wpos);
        MoveTo(wpos);
        if (wandertime <= 0)
        {
            wpos = RandomNavSphere(transform.position, 10, -1);
            wandertime = Random.Range(2f, 3f);
        }
    }

    /// <summary>
    /// Moves the Object to the passed in position and then makes the object look around
    /// </summary>
    /// <param name="pos">The position vector to move to</param>
    public virtual void Investigate(Vector3 pos)
    {
        var aSources = GetComponents<AudioSource>();
        findsound = aSources[0];
        robotwalk = aSources[1];
        robotwalk.enabled = false;
        if (Mathf.Sqrt(Mathf.Pow(transform.position.x - pos.x, 2) + Mathf.Pow(transform.position.z - pos.z, 2)) > sightrange / 2)
        {
            RotateTo(pos);
            MoveTo(pos);
        }
        if (Mathf.Sqrt(Mathf.Pow(transform.position.x - pos.x, 2) + Mathf.Pow(transform.position.z - pos.z, 2)) <= sightrange / 2)
        {
            WanderLook(pos);
            ignorefootpirints = false;
            InvestigateTime -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Moves the Object in a more sporradic / Alert way then base movement
    /// </summary>
    public virtual void Alert() //subject to change
    {

    }

    /// <summary>
    /// Rotates the Object randomly to scan the area
    /// </summary>
    public virtual void LookAround()
    {
        ignorefootpirints = false;
        time -= Time.deltaTime;
        if (time <= 0)
        {
            random = Random.Range(0, 2);
            time = Random.Range(0.5f, 1f);
        }
        if (random == 0)
        {
            transform.RotateAround(transform.position,Vector3.up,1);
            CorrectHeading();
        }
        else
        {
            transform.RotateAround(transform.position,Vector3.up, -1);
            CorrectHeading();
        }
        transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    /// <summary>
    /// Investigate mode lookaround and wander
    /// </summary>
    public virtual void WanderLook(Vector3 pos)
    {
        ignorefootpirints = false;
        time -= Time.deltaTime;
        if (time <= 0)
        {
            random = Random.Range(0, 3);
            wpos = RandomNavSphere(pos, 5, -1);
            time = Random.Range(0.5f, 1f);
        }
        if (random == 0)
        {
            transform.RotateAround(transform.position, Vector3.up, 1);
			if(ManAnimator !=null)
			ManAnimator.SetBool("IsWalk", false);
            CorrectHeading();
        }
        else if(random == 1)
        {
            transform.RotateAround(transform.position, Vector3.up, -1);
            CorrectHeading();
			if(ManAnimator !=null)
			ManAnimator.SetBool("IsWalk", false);
        }
        else
        {
            RotateTo(wpos);
            MoveTo(wpos);
        }
        transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    #endregion

    #region MovementFuctions - All functions for enemy movement

    /// <summary>
    /// help random moving
    /// </summary>
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    /// <summary>
    /// Moves the Object to a postion overtime depending on it's speed and the passed in position
    /// </summary>
    /// <param name="pos">The position vector to move to</param>
    protected void MoveTo(Vector3 pos)
    {
		if(ManAnimator !=null)
		ManAnimator.SetBool("IsWalk", true);
        agent.destination = pos;
    }

    /// <summary>
    /// Rotates the Object to point towards the position passed in
    /// </summary>
    /// <param name="pos">The position vector to rotate to</param>
	protected void RotateTo(Vector3 pos)
    {
        Vector3 vectorToTarget = (pos - transform.position).normalized;
        if (vectorToTarget != Vector3.zero)
        {
            Quaternion lookdir = Quaternion.LookRotation(vectorToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookdir, .2f);
        }
        transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    /// <summary>
    /// Corrects the z rotation of the object to be within 0f - 360f
    /// </summary>
    protected void CorrectHeading()
    {
        if (heading < 0)
        {
            heading = 360f + heading;
        }
        else if (heading > 360)
        {
            heading = 360f - heading;
        }
    }

    #endregion

    #region OperationFunctions - All main functions

    /// <summary>
    /// Initializes when object is created
    /// </summary>
    public void Start () {
        speed = basespeed;
        CurrentMode = Modes.PatrolMode;
        isAlert = false;
        sightrange = basesightrange;
        FOV = baseFOV;
        coll = GetComponent<Collider>();
        seen = false;
        time = 0.5f;
        InvestigateTime = 15f;
        count = 0;
        EventManager.PassSounds += RecieveSounds;
        EventManager.PassStrings += RecieveStrings;
        Color beginColor = transform.GetComponent<SpriteRenderer>().color;
        beginColor.a = 0;
        transform.GetComponent<SpriteRenderer>().color = beginColor;
        robottime = 5f;
        robotplay = false;
        InitialSpotlightLoc = transform.GetChild(0).position;
        agent = GetComponent<NavMeshAgent>();
        IsGoingBack = false;
        ReachedPatrol = true;
        alerted = false;
        wpos = transform.position;
        wandertime = 2f;
    }

    /// <summary>
    /// Updates this object once per frame
    /// </summary>
    void Update ()
    {
        if (Paused)
        {
            if (agent != null)
            {
                agent.Stop();
            }
            RotateTo(Vector3.zero);
            return;
        }
        if (agent != null)
        {
            agent.Resume();
        }
        if(Adjust == false)
        {
            if (robotwalk != null && findsound != null)
            {
                robotwalk.volume = SoundManager.instance.SFX[1].volume;
                findsound.volume = SoundManager.instance.SFX[1].volume;
                Adjust = true;
            }
        }
        UpdateSightLineRange();
        CheckVisibility();
        SightDetection();
        switch (CurrentMode)
        {
            case Modes.PatrolMode:
                Patrol();
                break;
            case Modes.FollowMode:
                if (player != null)
                    Follow(player.transform.position);
                break;
            case Modes.WanderingMode:
                Wander();
                break;
            case Modes.InvestigateMode:
                Investigate(playerlastknown);
                break;
        }
        if (isAlert && !name.Contains("Camera"))
        {
            Alert();
            CurrentMode = Modes.WanderingMode;
            wpos = path[0].transform.position;
            alerted = true;
        }
        if (InvestigateTime <= 0)
        {
            if (alerted)
            {
                CurrentMode = Modes.WanderingMode;
            }
            else
            {
                CurrentMode = Modes.PatrolMode;
                IsGoingBack = true;
                ReachedPatrol = false;
            }
            InvestigateTime = Random.Range(13f, 15f);
        }
    }

    /// <summary>
    /// Updates the light range depending on the enemies current sight range and it's FOV
    /// </summary>
    void UpdateSightLineRange()
    {
        transform.GetChild(0).transform.GetComponent<Light>().range = sightrange * 2;
        transform.GetChild(0).transform.GetComponent<Light>().spotAngle = FOV/2;
        transform.GetChild(0).position = new Vector3(transform.GetChild(0).position.x, InitialSpotlightLoc.y, transform.GetChild(0).position.z);
    }

    /// <summary>
    /// Checks wether this enemy is visible and if not turns invisible
    /// </summary>
    void CheckVisibility()
    {
        if(!IsMenu)
        {
            if (!PlayerHasSeen)
            {
                Color beginColor = transform.GetComponent<SpriteRenderer>().color;
                beginColor.a = 0;
                transform.GetComponent<SpriteRenderer>().color = beginColor;
                transform.GetChild(0).GetComponent<Light>().enabled = false;
                return;
            }
            PlayerHasSeen = false;
        }
        else if(IsMenu)
        {
            Color beginColor = transform.GetComponent<SpriteRenderer>().color;
            beginColor.a = 1;
            transform.GetComponent<SpriteRenderer>().color = beginColor;
            transform.GetChild(0).GetComponent<Light>().enabled = true;
        }
        
    }

    /// <summary>
    /// changes the bool 'PlayerHasSeen' to true
    /// </summary>
    public void SeenByPlayer()
    {
        PlayerHasSeen = true;
    }

    /// <summary>
    /// Sends multiple rays in front of the Object to allow the Object to "see"
    /// </summary>
    public virtual void SightDetection()
    {
        bool foundplayer = false;
        float currentangle = FOV/2;
        Vector3 strt = transform.position + (transform.rotation * Vector3.forward * coll.bounds.extents.x);
        Vector3 dir = transform.rotation * Vector3.forward;
        while (currentangle >= -FOV/2)
        {
            Ray ray = new Ray(strt, Quaternion.Euler(0, 0, currentangle) * dir);
            RaycastHit hitInfo;
            Physics.Raycast(ray,out hitInfo,sightrange,4352);
            Debug.DrawRay(strt, Quaternion.Euler(0, currentangle, 0)*dir, Color.red);

            if(hitInfo.collider != null)
            {
                if (hitInfo.transform.tag == "Player" && hitInfo.transform.gameObject.GetComponent<SpriteRenderer>().color.a >= 1f)
                {
                    player = hitInfo.collider;
                    CurrentMode = Modes.FollowMode;
                    foundplayer = true;
                    seen = true;
					FoundPlayerFirstTime ();
                    SetLightColor(1,0,0,1);
                    findsound = GetComponent<AudioSource>();
                    findsound.enabled = true;
					Footprint = false;
                    break;
                }
                if (hitInfo.transform.tag == "Footprint" && !ignorefootpirints)
                {
                    if (hitInfo.transform.GetComponent<FootPrintInWaterScript>().CANBESEEN)
                    {
                        Footprint = true;
                        player = hitInfo.collider;
                        CurrentMode = Modes.FollowMode;
                        foundplayer = true;
                        seen = true;
                        SetLightColor(1, 1, 0, 1);
                        findsound = GetComponent<AudioSource>();
                        findsound.enabled = true;
                    }
                }
            }
            currentangle -= 1f;
        }
        if (!foundplayer && seen)
        {
            playerlastknown = player.transform.position;
			if(!Footprint)
			MakeGhost (player.gameObject);
            CurrentMode = Modes.InvestigateMode;
            random = 1;
            wpos = playerlastknown;
            SetLightColor(1, 1, 1, 1);
            findsound.enabled = false;
            seen = false;
        }
    }

	void FoundPlayerFirstTime()
	{
		if (FoundplayerS != null && TimerForFPS <= 0f)
		{
			GameObject thing = Instantiate (FoundplayerS) as GameObject;
			Vector3 One = new Vector3 (1f, 1f, 1f);
			thing.AddComponent<FoundPlayerSScript> ().WhereIs (this.gameObject.transform.position + One);
			TimerForFPS = .5f;
		} 
		else
			TimerForFPS -= Time.deltaTime;
	}

	void MakeGhost(GameObject Ghostpo)
	{
		Instantiate (GhostPlayerObj,player.transform.position,player.transform.rotation);
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
        if (obj.tag == "Player")
        {
            ignorefootpirints = true;
            player = obj;
            CurrentMode = Modes.FollowMode;
            if (Timer_HitPlayerPerSce <= 0f)
            {
                Damage_Player();
                Timer_HitPlayerPerSce = Max_Time_For_Hits;
            }
            else
            {
				if(ManAnimator !=null)
				ManAnimator.SetBool("IsHitting", false);
                Timer_HitPlayerPerSce -= Time.deltaTime;
            }
        }
        if(obj.tag == "Enemy")
            wpos = RandomNavSphere(transform.position, 5, -1);
    }

    /// <summary>
    /// Detects when an Object has stopped colliding with this Object
    /// </summary>
    /// <param name="obj">The Object that stopped colliding into this Object</param>
    void OnTriggerExit(Collider obj)
    {
        if (Paused)
        {
            return;
        }
        if (obj.tag == "Player")
        {
            playerlastknown = obj.transform.position;
            player = null;
            CurrentMode = Modes.InvestigateMode;
            ignorefootpirints = true;
        }
    }

    /// <summary>
    /// Recieves sounds from the Event Manager
    /// </summary>
    /// <param name="values"></param>
    public virtual void RecieveSounds(Sound[] values)
    {
        foreach (Sound sound in values)
        {
            Vector3 direction = (transform.position - sound.Position).normalized;
            if (direction.magnitude <= sound.Volume)
            {
                Ray ray = new Ray(sound.Position, direction);
                RaycastHit hitInfo;
                Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 4608);
                if (hitInfo.collider != null)
                {
                    if (hitInfo.collider.tag == "Enemy")
                    {
                        CurrentMode = Modes.InvestigateMode;
                        ignorefootpirints = true;
                        playerlastknown = sound.Position;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Recieves strings from the event manager
    /// </summary>
    /// <param name="values">the array of strings recieved</param>
    public virtual void RecieveStrings(string[] values)
    {
        foreach(string s in values)
        {
            switch (s)
            {
                case "pause":
                    Paused = true;
                    break;
                case "unpause":
                    Paused = false;
                    break;
            }
            if (s.StartsWith("CameraSawEnemy"))
            {
                string tempVec = s.Remove(0, 14);
                string[] strings = tempVec.Split(',');

                Vector3 playerpos = new Vector3(float.Parse(strings[0]), float.Parse(strings[1]), float.Parse(strings[2]));
                Vector3 direction = (transform.position - playerpos).normalized;
                Ray ray = new Ray(playerpos, direction);
                RaycastHit hitInfo;
                Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 4608);
                if (hitInfo.collider != null)
                {
                    if (hitInfo.collider.tag == "Enemy")
                    {
                        CurrentMode = Modes.InvestigateMode;
                        playerlastknown = playerpos;
                    }
                }
            }
            else if (s.StartsWith("PlayerGT"))
            {
                string tempVec = s.Remove(0, 8);
                string[] strings = tempVec.Split(',');

                Vector3 playerpos = new Vector3(float.Parse(strings[0]), float.Parse(strings[1]), float.Parse(strings[2]));
                Vector3 direction = (transform.position - playerpos).normalized;
                Ray ray = new Ray(playerpos, direction);
                RaycastHit hitInfo;
                Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 4608);
                if (hitInfo.collider != null)
                {
                    if (hitInfo.collider.tag == "Enemy")
                    {
                        CurrentMode = Modes.InvestigateMode;
                        playerlastknown = playerpos;
                    }
                }
            }
            else if (s.StartsWith("FullyAware") && alerted != true)
            {
                isAlert = true;
            }
        }
    }

    void OnDestroy()
    {
        EventManager.PassSounds -= RecieveSounds;
        EventManager.PassStrings -= RecieveStrings;
    }

    #endregion

    #region HelpFunctions - All helping functions
    /// <summary>
    /// Sets the Color of the sight lines in front of the enemy
    /// </summary>
    /// <param name="red"></param>
    /// <param name="blue"></param>
    /// <param name="green"></param>
    /// <param name="alpha"></param>
    protected void SetLightColor(float red, float blue, float green, float alpha)
    {
        Color c = transform.GetChild(0).transform.GetComponent<Light>().color;
        c.a = alpha;
        c.r = red;
        c.g = green;
        c.b = blue;
        transform.GetChild(0).transform.GetComponent<Light>().color = c;
    }

    /// <summary>
    /// Help security camera telling other enemy where is  player
    /// </summary>
    public void SetLoastKnowPosition(Vector3 pos)
    {
        playerlastknown = pos;
    }
    /// <summary>
    /// Help security camera get the postion
    /// </summary>
    public Vector3 Getplayerlastknowpostion()
    {
        return playerlastknown;
    }

    /// <summary>
    /// Help security camera telling other enemy where is player current postion
    /// </summary>
    public void SetPlayer(Collider _player)
    {
        player = _player;
    }

	/// <summary>
	/// Damages the player based on this enemies damage value.
	/// </summary>
	public void Damage_Player()
	{	
		string[] toBePassed={string.Format("Damage{0}", Damage_amount)};
		EventManager.StringEvent (toBePassed);
		PushBack();
		if (ManAnimator != null) 
		{
			ManAnimator.SetBool("IsHitting", true);
		}
	}

	void PushBack()
	{
		//this.gameObject.
		Vector3 Try=this.gameObject.transform.position;
		float Xpos = Mathf.Round(Try.x);
		float Ypos = Mathf.Round(Try.y);
		float Zpos = Mathf.Round(Try.z);
		string NameTag = "PushBack";
		string topass = NameTag + "X" + Xpos.ToString ();// +"Y"+ Ypos.ToString () +"Z"+ Zpos.ToString ();
		string[] To = { string.Format (topass) };
		EventManager.StringEvent (To);
		topass = NameTag +"Y"+ Ypos.ToString ();
		string[] Be = { string.Format (topass) };
		EventManager.StringEvent (Be);
		topass = NameTag +"Z"+ Zpos.ToString ();
		string[] Pass = { string.Format (topass) };
		EventManager.StringEvent (Pass);
	}

	void SetDamage(float dam)
	{
		Damage_amount = dam;
	}

    #endregion
}
