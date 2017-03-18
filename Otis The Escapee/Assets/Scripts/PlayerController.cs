using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using JetBrains.Annotations;
//using UnityEditor.SceneManagement;

/// <summary>
/// List of reusableObjects
/// </summary>
public enum reusableObjects { hatchet, none }
/// <summary>
/// List of singleUseObjects
/// </summary>
public enum singleUseObjects { pebble, healthkit, none , dummy}


public class PlayerController : MonoBehaviour
{
    #region Serializable Property Drawers
    [Serializable]
    class bulletAtribs
    {    /// <summary>
         /// The prefab for a bullet (eventaully replace with rock)
         /// </summary>
        public GameObject bullet;
        /// <summary>
        /// full length of time before rock can be thrown again
        /// </summary>
        public float bulletCooldownLength = 1f;
        /// <summary>
        /// Current time of rock cooldown
        /// </summary>
        public float bulletCooldown;
    }

    [Serializable]
    class movementAtribs
    {
        /// <summary>
        /// speed the character moves when walking
        /// </summary>
        public float walkingSpeed = 5f;
        /// <summary>
        /// speed changer for diferent actions
        /// </summary>
        public float speedMult = 1f;
        /// <summary>
        /// Determines how long until the player makes a sound when running
        /// </summary>
        public float RunSound = 0.5f;
    }

    [Serializable]
    class StaminaAtribs
    {
        /// <summary>
        /// Amount of time before water can be re-entered after running out of stamina in the water
        /// </summary>
        public float waterCooldownLength = 2f;
        /// <summary>
        /// Amount of time before stamina can be changed
        /// </summary>
        public float staminaCooldownLength = .1f;
        /// <summary>
        /// Suggested number to change stamina
        /// </summary>
        public float staminaUse = 1f;
        /// <summary>
        /// Actual timer for stamina cooldown before change
        /// </summary>
        public float staminaCooldown;
        /// <summary>
        /// Actual timer for water cooldown before re-enter
        /// </summary>
        public float waterCooldown;
        /// <summary>
        /// Don't use this stamina storage variable
        /// </summary>
        private float __DONTUSETHIS__stamina;
        /// <summary>
        /// Current stamina of player
        /// </summary>
        public float Stamina
        {
            get
            {
                return __DONTUSETHIS__stamina;
            }

            set
            {
                __DONTUSETHIS__stamina = Mathf.Clamp(value, 0.0f, 100f);
                string[] toBePassed = { string.Format("stamina{0}", __DONTUSETHIS__stamina) };
                EventManager.StringEvent(toBePassed);
            }
        }
    }

    [Serializable]
    class Inventory
    {
        /// <summary>
        /// Determines if the character has a key or not
        /// </summary>
        public bool hasKey;
        /// <summary>
        /// What reusableObject the character has if any (can be none / null)
        /// </summary>
        public reusableObjects reusableObject = reusableObjects.none;
        /// <summary>
        /// The single use object that the character has if any (can be null)
        /// </summary>
        public singleUseObjects singleUseObject = singleUseObjects.none;
        /// <summary>
        /// The gameobject that gets placed down when having the dummy item in inventory
        /// </summary>
        [SerializeField] public GameObject dummy;
    }

    [Serializable]
    class FeedbackAtribs
    {
        /// <summary>
        /// The flashing bool tells when player needs to flash
        /// </summary>
        public bool Flashing_Bool;

        /// <summary>
        /// The cur num of times player flashed red
        /// </summary>
        public short NumOfTimeflashed;

        /// <summary>
        /// The max number of times player flashed red
        /// </summary>
        public short Max_NumOfTimes = 2;

        /// <summary>
        /// The haft A sec.  Just a little helper
        /// </summary>
        public float HaftASec = .3f;

        /// <summary>
        /// The current timer for flashing.
        /// </summary>
        public float CurTimerForFlash;

        public Color initialColor;

        public Color secondaryColor;
    }
    #endregion

    #region Variables
    /// <summary>
    /// floatVoid delegate
    /// </summary>
    /// <returns></returns>
    private delegate float floatVoid();
    /// <summary>
    /// Controls what happens when sprint key is pressed
    /// </summary>
    /// <returns></returns>
    private floatVoid onSpace;

    [SerializeField] private int LevelIdentifier = 0;

    [SerializeField] private bulletAtribs thrownItem = new bulletAtribs();

    [SerializeField] private movementAtribs movement = new movementAtribs();

    [SerializeField] private StaminaAtribs Stamina = new StaminaAtribs();

    [SerializeField] private Inventory inventory = new Inventory();

    [SerializeField] private FeedbackAtribs feedback = new FeedbackAtribs();

    [SerializeField] private Animator PlayerAnimator;

    /// <summary>
    /// The current tree the character can interact with
    /// </summary>
    private GameObject CurrTree;

    /// <summary>
    /// Whether the player is paused by the console or not
    /// </summary>
    private bool consolePause;

    /// <summary>
    /// Pauses the game;
    /// </summary>
    private bool pause = false;

    private bool canPause = false;
    /// <summary>
	/// The keys that will be used for player input.
	/// </summary>
    protected Dictionary<string, KeyCode> Keys = new Dictionary<string, KeyCode>();
    public bool outOfStamina = false;
	private Vector3 KnockBack;
	private bool KnockBackBool = false;
	private float KnockBackTimer = 0f;
	private short NumKnock = 0;
	private short MaxNumKnock = 3;
	private float KnockX;
	private float KnockY;
	private float KnockZ;
	protected float heading;
	[SerializeField]
	string HealthpackMess_ONLY;
	[SerializeField]
	string WaterDropMess_ONLY;

    private bool startEnd = false;
    private float startEndTimer = 1f;
    #endregion

    #region Health
    /// <summary>
    /// Don't use this health storage variable
    /// </summary>
    private float __DONTUSETHIS__health;
    /// <summary>
    /// Current health of the player
    /// </summary>
    private float Health
    {
        get { return __DONTUSETHIS__health; }
        set
        {
            __DONTUSETHIS__health = Mathf.Clamp(value, 0.0f, 100f);
            string[] toBePassed = { string.Format("health{0}", __DONTUSETHIS__health) };
            EventManager.StringEvent(toBePassed);
            if (__DONTUSETHIS__health <= 0.1f)
            {
                PlayerAnimator.SetBool("IsDead", true);
            }
        }
    }
	private void Damage_Player(float dam)
	{
		if (dam > 0.0f)
		{
			Health -= dam;

			string BleedOut = "StartBleed";
			string[] Pass = { string.Format (BleedOut) };
			EventManager.StringEvent (Pass);
		}
	}
    #endregion

    #region Stamina
    /// <summary>
    /// Regenerates stamina
    /// </summary>
    /// <param name="amt">amount of stamina to regain</param>
    private void Regain_S(float amt)
    {
        if (Stamina.staminaCooldown <= 0.0f)
        {
            Stamina.Stamina += amt;
            Stamina.staminaCooldown = Stamina.staminaCooldownLength;
        }
    }

    /// <summary>
    /// Reduces Stamina
    /// </summary>
    /// <param name="amt">amount of stamina to lose</param>
	private void Use_S(float amt)
    {
        if (Stamina.staminaCooldown <= 0.0f)
        {
            Stamina.Stamina -= amt;
            Stamina.staminaCooldown = Stamina.staminaCooldownLength;
        }
    }
    #endregion

    #region OverallAwarness
    /// <summary>
    /// float value that determines the areas awarness level towards this player
    /// </summary>
    private float OverallAwarness;
    /// <summary>
    /// Changes the OverallAwarness towards this player
    /// </summary>
    /// <param name="value">amount to set it as</param>
    public void SetOverallAwarness(float value)
    {
        OverallAwarness = value;
    }
    /// <summary>
    /// Gets the overallAwarness towards this player
    /// </summary>
    /// <returns></returns>
    public float GetOverallAwarness()
    {
        return OverallAwarness;
    }
    /// <summary>
    /// Updates the Overall Alertness and Updates other things depending on it
    /// </summary>
    /// <param name="value">value to add or subtract the awarness by</param>
    private void UpdateAwareness(float value)
    {
        SetOverallAwarness(OverallAwarness + value);
        if (OverallAwarness > 100)
        {
            OverallAwarness = 100;
            string[] WillBePassed = { "FullyAware" };
            EventManager.StringEvent(WillBePassed);
        }
        float Vp = OverallAwarness * .01f;
        string[] ToBePassed = { string.Format("VPower{0}", Vp) };
        EventManager.StringEvent(ToBePassed);
        string[] Passing = { "VColor" + 0.5 + "," + 0 + "," + 0 + "," + 1 };
        EventManager.StringEvent(Passing);
    }
    #endregion

    #region Core Functionality
    void Start ()
    {
        feedback.initialColor = feedback.initialColor == null ? Color.white : feedback.initialColor;
        feedback.secondaryColor = feedback.secondaryColor == null ? Color.red : feedback.secondaryColor;
        thrownItem.bullet = thrownItem.bullet ?? new GameObject();
        Keys.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W")));
        Keys.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
        Keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
        Keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
        Keys.Add("Sprint", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Sprint", "Space")));
        Keys.Add("Reusable", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Reusable", "E")));
        Keys.Add("Consumable", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Consumable", "R")));
        inventory.hasKey = false;
        inventory.singleUseObject = singleUseObjects.none;
	    EventManager.PassStrings += ParseStrings;
	    onSpace = sprint;
        Stamina.Stamina += 100f;
        Health += 100f;
        if (PlayerPrefs.HasKey("FileToLoadFrom"))
        {
            int file = PlayerPrefs.GetInt("FileToLoadFrom");
            string[] toBePassed = {string.Format("Load{0}", file)};
            PlayerPrefs.DeleteKey("FileToLoadFrom");
            EventManager.StringEvent(toBePassed);
        }
        if (PlayerPrefs.HasKey("PlayerSUO"))
        {
            int SUO = PlayerPrefs.GetInt("PlayerSUO");
            switch (SUO)
            {
                case 1:
                {
                    SetSingleUse(singleUseObjects.pebble);
                    break;
                }
                case 2:
                {
                    SetSingleUse(singleUseObjects.healthkit);
                    break;
                }
            }
            PlayerPrefs.DeleteKey("PlayerSUO");
        }
        PlayerPrefs.Save();
	}

    /// <summary>
    /// Ticks down all player cooldowns (timers)
    /// </summary>
    void UpdateCooldowns()
    {
        Stamina.staminaCooldown = Stamina.staminaCooldown >= 0.0f ? Stamina.staminaCooldown - Time.deltaTime : -1f;
        thrownItem.bulletCooldown = thrownItem.bulletCooldown >= 0.0f ? thrownItem.bulletCooldown - Time.deltaTime : -1f;
        Stamina.waterCooldown = Stamina.waterCooldown >= 0.0f ? Stamina.waterCooldown - Time.deltaTime : -1f;
        if (onSpace == sprint)
        {
            movement.RunSound -= Time.deltaTime;
        }
        if (startEnd)
        {
            startEndTimer -= Time.deltaTime;
        }
    }

    void Update()
    {
        PlayerAnimator.SetBool("ChopTree", false);
        if (pause)
        {
            return;
        }
        if (canPause && (Input.GetKeyDown(KeyCode.Escape) || Input.GetAxis("StartButton") > 0))
        {
                string[] toBePassed = { "pause", "ShowPause" };
                EventManager.StringEvent(toBePassed);
        }
        canPause = true;
        EnemyCheckVisible();
        UpdateCooldowns();
        if (!consolePause)
        {
            movement.speedMult = onSpace();
            transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            CheckForHealth();
            Vector3 MoveDirection = Vector3.zero;
            MoveDirection.z = -Input.GetAxis("LeftJoystickVertical");
            MoveDirection.x = Input.GetAxis("LeftJoystickHorizontal");
            if(Input.GetKey(Keys["Up"]))
            {
                MoveDirection.z = 1;
            }
            else if (Input.GetKey(Keys["Down"]))
            {
                MoveDirection.z = -1;
            }
            if(Input.GetKey(Keys["Left"]))
            {
                MoveDirection.x = -1;
            }
            else if (Input.GetKey(Keys["Right"]))
            {
                MoveDirection.x = 1;
            }
            MoveDirection.Normalize();
            if ((MoveDirection != Vector3.zero && onSpace != inWater) || (MoveDirection != Vector3.zero && onSpace == inWater && transform.GetComponent<SpriteRenderer>().color.a >= 1f))
            {
                RotateTo(transform.position + MoveDirection*(movement.walkingSpeed*movement.speedMult)*Time.deltaTime);
                PlayerAnimator.SetBool("IsWalking", true);
            }
            else
            {
                PlayerAnimator.SetBool("IsWalking", false);
            }
            PlayerAnimator.SetFloat("Speed", movement.walkingSpeed * movement.speedMult);
            transform.Translate(MoveDirection * (movement.walkingSpeed * movement.speedMult) * Time.deltaTime, Space.World);
            if (Input.GetKey(Keys["Consumable"]) || Input.GetAxis("AButton") > 0)
            {
                switch (inventory.singleUseObject)
                {
                    case singleUseObjects.pebble:
                        {
                            if (thrownItem.bulletCooldown <= 0f)
                            {
                                Instantiate(thrownItem.bullet, transform.position, transform.rotation);
                                thrownItem.bulletCooldown = thrownItem.bulletCooldownLength;
                                SetSingleUse(singleUseObjects.none);
                            }
                            break;
                        }
                    case singleUseObjects.healthkit:
                        {
                            Health += 50f;
						string[] toBePassed = {"SUO1"};
						string[] BePassed = {string.Format (HealthpackMess_ONLY)};
						EventManager.StringEvent(BePassed);
                            SetSingleUse(singleUseObjects.none);
                            break;
                        }
                    case singleUseObjects.dummy:
                        {
                            Instantiate(inventory.dummy, transform.position, transform.rotation);
                            SetSingleUse(singleUseObjects.none);
                            break;
                        }
                    case singleUseObjects.none:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            if (Input.GetKey(Keys["Reusable"]) || Input.GetAxis("BButton") > 0)
            {
                switch (inventory.reusableObject)
                {
                    case reusableObjects.hatchet:
                        {
                            PlayerAnimator.SetBool("ChopTree", true);
                            if (CurrTree != null)
                            {
                                CurrTree.GetComponent<TreeScript>().CutTree();
                                CurrTree = null;
                                string[] toBePassed = { "unusableHatchet" };
                                EventManager.StringEvent(toBePassed);
                            }
                            break;
                        }
                    case reusableObjects.none:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            if (feedback.Flashing_Bool)
                //Flashing();
                if (KnockBackBool)
                {
                    //PushBack ();
                }
        }
        if (startEndTimer <= 0.0f)
        {
            SceneManager.LoadScene("EndScene");
        }
    }

    void CheckForHealth()
    {
        if(Health <= 0)
        {           
            for (int i = 0; i < SoundManager.instance.SFX.Length; i++)
            {
                SoundManager.instance.SFX[0].Stop();
            }
            if(Application.loadedLevelName == "Level1")
            SceneManager.LoadScene("LoseGameScene");
            else if (Application.loadedLevelName == "Level2")
                SceneManager.LoadScene("LoseGameScene2");
            else if (Application.loadedLevelName == "Level3")
                SceneManager.LoadScene("LoseGameScene3");
        }
    }

    /// <summary>
    /// Checks for enemies that are in visible range and switches their visibility on
    /// </summary>
    void EnemyCheckVisible()
    {
        Vector3 dir = transform.rotation*Vector3.up;
        for (int i = 0; i < 360; i++)
        {
            Ray ray = new Ray(transform.position, Quaternion.Euler(0, i, 0)*dir);
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, float.MaxValue, 20992);
            if (hitInfo.transform != null)
            {
                switch (hitInfo.transform.tag)
                {
                    case "Enemy":
                        Color c = hitInfo.transform.GetComponent<SpriteRenderer>().color;
                        c.a = 1;
                        hitInfo.transform.GetComponent<SpriteRenderer>().color = c;
                        hitInfo.transform.GetComponent<BaseEnemyController>().SeenByPlayer();
                        hitInfo.transform.GetComponentsInChildren<Light>()[0].enabled = true;
                        break;
                    case "Key":
                        hitInfo.transform.GetComponent<SpriteRenderer>().enabled = true;
                        break;
                }
            }
        }
    }

    void OnDestroy()
    {
        EventManager.PassStrings -= ParseStrings;
    }

    /// <summary>
    /// Sprint functionallity when not in water
    /// </summary>
    /// <returns></returns>
    float sprint()
    {
        if ((Input.GetKey(Keys["Sprint"]) || Input.GetAxis("LeftJoystickPress") > 0) && Stamina.Stamina > 0.0f)
        {
            if (Input.GetKey(Keys["Up"]) || Input.GetKey(Keys["Right"]) || Input.GetKey(Keys["Down"]) || Input.GetKey(Keys["Left"]) || (Input.GetAxis("LeftJoystickHorizontal") != 0) || (Input.GetAxis("LeftJoystickVertical") != 0))
            {
                Use_S(Stamina.staminaUse);
                if (movement.RunSound <= 0.0f)
                {
                    movement.RunSound = 0.5f;
                    Sound sound = new Sound
                    {
                        Position = transform.position,
                        Volume = 40f
                    };
                    Sound[] toBePassed = { sound };
                    EventManager.SoundEvent(toBePassed);
                }
            }
            else
            {
                Regain_S(Stamina.staminaUse);
            }
            return 2;
        }
        if ((Input.GetKey(Keys["Sprint"]) || Input.GetAxis("LeftJoystickPress") > 0) && Stamina.Stamina <= 0.0f)
        {
            outOfStamina = true;
            StartCoroutine(FreezeStamina(5.0f));
            return 1;
        }
        if(outOfStamina == false)
        {
            Regain_S(Stamina.staminaUse);
        }
        return 1;
    }

    IEnumerator FreezeStamina(float time)
    {
        yield return new WaitForSeconds(time);
        outOfStamina = false;
        Regain_S(Stamina.staminaUse);
    }

    /// <summary>
    /// Hiding functionality when in water
    /// </summary>
    /// <returns></returns>
    float inWater()
    {
        if ((Input.GetKey(Keys["Sprint"]) || Input.GetAxis("LeftJoystickPress") > 0) && Stamina.waterCooldown <= 0f)
        {

            if (Stamina.Stamina == 0)
            {
                Stamina.waterCooldown = Stamina.waterCooldownLength;
            }
            Use_S(Stamina.staminaUse);
            Hide();
            return 0f;
        }
        else
        {
            Regain_S(Stamina.staminaUse);
            UnHide();
            return .5f;
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Tree")
        {
            if (inventory.reusableObject == reusableObjects.hatchet)
            {
                string[] toBePassed = { "MUO1" };
                EventManager.StringEvent(toBePassed);
                CurrTree = coll.gameObject;
            }
        }
        if (coll.gameObject.tag == "Door" || coll.gameObject.tag == "WinPath")
        {
            if (coll.gameObject.tag == "WinPath")
            {
                SaveUsables();
                for (int i = 0; i < SoundManager.instance.SFX.Length; i++)
                {
                    SoundManager.instance.SFX[i].Stop();
                }
                if(Application.loadedLevelName == "Level1")
                {
                    SceneManager.LoadScene("WinGameScene");
                }
                else if(Application.loadedLevelName == "Level2")
                {
                    SceneManager.LoadScene("WinGameScene2");
                }
            }
            else if(inventory.hasKey)
            {
                for (int i = 0; i < SoundManager.instance.SFX.Length; i++)
                {
                    SoundManager.instance.SFX[i].Stop();
                }
                coll.GetComponent<Animator>().SetTrigger("Open");
                startEnd = true;
                SceneManager.LoadScene("EndScene");
            }
        }
        if (coll.gameObject.tag == "Dummy")
        {
            if (inventory.singleUseObject == singleUseObjects.none)
            {
                coll.gameObject.GetComponent<ItemPickUpScript>().DestroyObject();
                SetSingleUse(singleUseObjects.dummy);
            }
        }
        if (coll.gameObject.tag == "Pebble")
        {
            if (inventory.singleUseObject == singleUseObjects.none)
            {
                coll.gameObject.GetComponent<ItemPickUpScript>().DestroyObject();
                SetSingleUse(singleUseObjects.pebble);
            }
        }
        if (coll.gameObject.tag == "HealthKit")
        {
            if (inventory.singleUseObject == singleUseObjects.none)
            {
                coll.gameObject.GetComponent<ItemPickUpScript>().DestroyObject();
                SetSingleUse(singleUseObjects.healthkit);
            }
        }
    }

    void OnTriggerStay(Collider coll)
    {
        if (coll.tag == "Tree")
        {
            if (inventory.reusableObject == reusableObjects.hatchet)
            {
                string[] toBePassed = { "MUO1" };
                EventManager.StringEvent(toBePassed);
                CurrTree = coll.gameObject;
            }
        }
        switch (coll.tag)
        {
            case "Water":
                movement.speedMult = .5f;
                onSpace = inWater;
                break;
            case "Bush":
                Hide();
                break;
            case "Cave":
                Hide();
                break;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        switch (coll.tag)
        {
            case "Tree":
                if (inventory.reusableObject == reusableObjects.hatchet)
                {
                    string[] toBePassed = { "MUO0" };
                    EventManager.StringEvent(toBePassed);
                    CurrTree = null;
                }
                break;
		case "Water":
			UnHide ();
			onSpace = sprint;
			string[] Mess ={string.Format(WaterDropMess_ONLY)};
			EventManager.StringEvent(Mess);
                break;
            case "Bush":
                UnHide();
                break;
            case "Cave":
                UnHide();
                break;
        }
    }

    /// <summary>
    /// Gives distance between players current position and an inputed one
    /// </summary>
    /// <param name="pos">position to check distance against</param>
    /// <returns></returns>
    float CheckDistance(Vector3 pos)
    {
        return (transform.position - pos).magnitude;
    }

    /// <summary>
    /// Makes the character hidden to enemies
    /// </summary>
    void Hide()
    {
        var component = GetComponent<SpriteRenderer>();
        Color tempColor = component.color;
        tempColor = new Color(0, 0, 0, .5f);
        component.color = tempColor;
    }

    /// <summary>
    /// Makes the character see-able by enemies
    /// </summary>
    void UnHide()
    {
        var component = GetComponent<SpriteRenderer>();
        var tempColor = component.color;
        tempColor = new Color(1, 1, 1, 1);
        component.color = tempColor;
    }

    /// <summary>
    /// Changes the state of whether the player currently holds the key
    /// </summary>
    /// <param name="newState"></param>
    public void SetKey(bool newState)
    {
        if (newState)
        {
            string[] toBePassed = {"KEY0"};
            EventManager.StringEvent(toBePassed);
            inventory.hasKey = true;
        }
        else
        {
            string[] toBePassed = {"KEY-1"};
            EventManager.StringEvent(toBePassed);
            inventory.hasKey = false;
        }
    }

    /// <summary>
    /// Changes the state of wether the player currently holds the hatchet
    /// </summary>
    /// <param name="type"></param>
    public void SetReusable(reusableObjects type)
    {
        switch (type)
        {
            case reusableObjects.hatchet:
            {
                string[] toBePassed = {"MUO0"};
                EventManager.StringEvent(toBePassed);
                inventory.reusableObject = reusableObjects.hatchet;
                break;
            }
            case reusableObjects.none:
            {
                string[] toBePassed = {"MUO-1"};
                EventManager.StringEvent(toBePassed);
                inventory.reusableObject = reusableObjects.none;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetSingleUse(singleUseObjects type)
    {
        switch (type)
        {
            case singleUseObjects.pebble:
            {
                string[] toBePassed = {"SUO0"};
                EventManager.StringEvent(toBePassed);
                inventory.singleUseObject = singleUseObjects.pebble;
                break;
            }
            case singleUseObjects.healthkit:
            {
				string[] toBePassed = {"SUO1"};
				EventManager.StringEvent(toBePassed);
				inventory.singleUseObject = singleUseObjects.healthkit;
                break;
            }
            case singleUseObjects.none:
            {
                string[] toBePassed = {"SUO-1"};
                EventManager.StringEvent(toBePassed);
                inventory.singleUseObject = singleUseObjects.none;
                break;
            }
            case singleUseObjects.dummy:
                {
                    string[] toBePassed = { "SUO2" };
                    EventManager.StringEvent(toBePassed);
                    inventory.singleUseObject = singleUseObjects.dummy;
                    break;
                }
        }
    }

    /// <summary>
    /// Recieves events from Event Manager to affect player respectivly
    /// </summary>
    /// <param name="values"></param>
    private void ParseStrings(string[] values)
    {
        foreach (string s in values)
        {
            switch (s)
            {
                case "giveHatchet":
                    SetReusable(reusableObjects.hatchet);
                    break;
                case "takeHatchet":
                    SetReusable(reusableObjects.none);
                    break;
                case "giveKey":
                    SetKey(true);
                    break;
                case "takeKey":
                    SetKey(false);
                    break;
                case "inputFalse":
                    consolePause = true;
                    break;
                case "inputTrue":
                    consolePause = false;
                    break;
                case "pause":
                    pause = true;
                    break;
                case "unpause":
                    pause = false;
                    break;
            }
            if (s.StartsWith("Aware"))
            {
                string temp = s.Remove(0, 5);
                UpdateAwareness(float.Parse(temp));
            }
            if (s.StartsWith("PSpeed")) //example syntax PSpeed1.5
            {
                string temp = s.Remove(0, 6);
                movement.walkingSpeed = float.Parse(temp);
            }
            else if (s.StartsWith("save"))
            {
                string temp = s.Remove(0, 4);
                Save(int.Parse(temp));
            }
            else if (s.StartsWith("load"))
            {
                string temp = s.Remove(0, 4);
                Load(int.Parse(temp));
            }
            else if (s.StartsWith("Load"))
            {
                string temp = s.Remove(0, 4);
                if (PlayerPrefs.GetInt(string.Format("LevelFile{0}", int.Parse(temp))) == LevelIdentifier)
                {
                    string[] toBePassed = {string.Format("load{0}", LevelIdentifier)};
                    EventManager.StringEvent(toBePassed);
                }
            }
            else if (s.StartsWith("delete"))
            {
                string temp = s.Remove(0, 6);
                int value;
                if (!int.TryParse(temp, out value))
                {
                    Debug.Log("Could not parse int from " + temp);
                }
                else delete(value);
            }
            else if (s.StartsWith("Damage"))
            {
                string temp = s.Remove(0, 6);
                Damage_Player(float.Parse(temp));
                feedback.Flashing_Bool = true;
            }
            else if (s.StartsWith("PushBack"))
            {
                string temp = s.Remove(0, 8);
                string PosSpot = temp[0].ToString();
                switch (PosSpot)
                {
                    case "X":
                    {
                        temp = temp.Remove(0, 1);
                        KnockX = float.Parse(temp);
                        break;
                    }
                    case "Y":
                    {
                        temp = temp.Remove(0, 1);
                        KnockY = float.Parse(temp);
                        break;
                    }
                    case "Z":
                    {
                        temp = temp.Remove(0, 1);
                        KnockZ = float.Parse(temp);
                        KnockBack = new Vector3(KnockX, KnockY, KnockZ);
                        KnockBackBool = true;
                        break;
                    }
                }
            }
        }
    }

    private void Save(int fileNumber)
    {
        PlayerPrefs.SetFloat(string.Format("PlayerPosXFile{0}", fileNumber), transform.position.x);
        PlayerPrefs.SetFloat(string.Format("PlayerPosYFile{0}", fileNumber), transform.position.y);
        PlayerPrefs.SetFloat(string.Format("PlayerPosZFile{0}", fileNumber), transform.position.z);
        PlayerPrefs.SetFloat(string.Format("PlayerHealthFile{0}", fileNumber), Health);
        PlayerPrefs.SetFloat(string.Format("PlayerStaminaFile{0}", fileNumber), Stamina.Stamina);
        PlayerPrefs.SetFloat(string.Format("PlayerRotXFile{0}", fileNumber), transform.rotation.x);
        PlayerPrefs.SetFloat(string.Format("PlayerRotYFile{0}", fileNumber), transform.rotation.y);
        PlayerPrefs.SetFloat(string.Format("PlayerRotZFile{0}", fileNumber), transform.rotation.z);
        PlayerPrefs.SetFloat(string.Format("PlayerRotWFile{0}", fileNumber), transform.rotation.w);
        PlayerPrefs.SetInt(string.Format("PlayerHasKeyFile{0}", fileNumber), inventory.hasKey ? 1 : 0);
        PlayerPrefs.SetInt(string.Format("LevelFile{0}", fileNumber), LevelIdentifier);
        switch (inventory.reusableObject)
        {
            case reusableObjects.hatchet:
                PlayerPrefs.SetString(string.Format("PlayerReusableObjectFile{0}", fileNumber), "hatchet");
                break;
            case reusableObjects.none:
                PlayerPrefs.SetString(string.Format("PlayerReusableObjectFile{0}", fileNumber), "");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        switch (inventory.singleUseObject)
        {
            case singleUseObjects.pebble:
                PlayerPrefs.SetString(string.Format("PlayerSingleUseObjectFile{0}", fileNumber), "pebble");
                break;
            case singleUseObjects.healthkit:
                PlayerPrefs.SetString(string.Format("PlayerSingleUseObjectFile{0}", fileNumber), "healthkit");
                break;
            case singleUseObjects.none:
                PlayerPrefs.SetString(string.Format("PlayerSingleUseObjectFile{0}", fileNumber), "");
                break;
            case singleUseObjects.dummy:
                PlayerPrefs.SetString(string.Format("PlayerSingleUseObjectFile{0}", fileNumber), "dummy");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        PlayerPrefs.Save();
    }

    private void Load(int fileNumber)
    {
        Vector3 pos = new Vector3()
        {
            x = PlayerPrefs.GetFloat(string.Format("PlayerPosXFile{0}", fileNumber)), y = PlayerPrefs.GetFloat(string.Format("PlayerPosYFile{0}", fileNumber)), z = PlayerPrefs.GetFloat(string.Format("PlayerPosZFile{0}", fileNumber))
        };
        Quaternion rot = new Quaternion
        {
            w = PlayerPrefs.GetFloat(string.Format("PlayerRotWFile{0}", fileNumber)), x = PlayerPrefs.GetFloat(string.Format("PlayerRotXFile{0}", fileNumber)), y = PlayerPrefs.GetFloat(string.Format("PlayerRotYFile{0}", fileNumber)), z = PlayerPrefs.GetFloat(string.Format("PlayerRotZFile{0}", fileNumber))
        };
        transform.position = pos;
        transform.rotation = rot;
        Health = PlayerPrefs.GetFloat(string.Format("PlayerHealthFile{0}", fileNumber));
        Stamina.Stamina = PlayerPrefs.GetFloat(string.Format("PlayerStaminaFile{0}", fileNumber));
        inventory.hasKey = PlayerPrefs.GetInt(string.Format("PlayerHasKeyFile{0}", fileNumber)) == 1;
        switch (PlayerPrefs.GetString(string.Format("PlayerReusableObjectsFile{0}", fileNumber)))
        {
            case "hatchet":
                SetReusable(reusableObjects.hatchet);
                break;
            case "":
                SetReusable(reusableObjects.none);
                break;
        }
        switch (PlayerPrefs.GetString(string.Format("PlayerSingleUseObjectFile{0}", fileNumber)))
        {
            case "pebble":
                SetSingleUse(singleUseObjects.pebble);
                break;
            case "healthkit":
                SetSingleUse(singleUseObjects.healthkit);
                break;
            case "dummy":
                SetSingleUse(singleUseObjects.dummy);
                break;
            case "":
                SetSingleUse(singleUseObjects.none);
                break;
        }
    }

    private void delete(int fileNumber)
    {
        PlayerPrefs.DeleteKey(string.Format("PlayerPosXFile{0}", fileNumber));
        PlayerPrefs.DeleteKey(string.Format("PlayerPosYFile{0}", fileNumber));
        PlayerPrefs.DeleteKey(string.Format("PlayerPosZFile{0}", fileNumber));
        PlayerPrefs.DeleteKey(string.Format("PlayerHealthFile{0}", fileNumber));
        PlayerPrefs.DeleteKey(string.Format("PlayerStaminaFile{0}", fileNumber));
        PlayerPrefs.DeleteKey(string.Format("PlayerRotXFile{0}", fileNumber));
        PlayerPrefs.DeleteKey(string.Format("PlayerRotYFile{0}", fileNumber));
        PlayerPrefs.DeleteKey(string.Format("PlayerRotZFile{0}", fileNumber));
        PlayerPrefs.DeleteKey(string.Format("PlayerRotWFile{0}", fileNumber));
        PlayerPrefs.DeleteKey(string.Format("PlayerHasKeyFile{0}", fileNumber));
        PlayerPrefs.DeleteKey(string.Format("PlayerReusableObjectsFile{0}", fileNumber));
        PlayerPrefs.DeleteKey(string.Format("LevelFile{0}", fileNumber));
        PlayerPrefs.DeleteKey(string.Format("PlayerSingleUseObjectFile{0}", fileNumber));
        PlayerPrefs.Save();
    }

    private void Flashing()
    {
        if (feedback.Flashing_Bool)
        {
            if (feedback.CurTimerForFlash >= feedback.HaftASec)
            {
                Color MyC = gameObject.GetComponent<SpriteRenderer>().color;
                switch (MyC.Equals(feedback.secondaryColor))
                {
                    case true:
                    {
                        if (gameObject.GetComponent<SpriteRenderer>().color.a == 1f)
                            gameObject.GetComponent<SpriteRenderer>().color = feedback.initialColor;
                        if (feedback.NumOfTimeflashed >= feedback.Max_NumOfTimes)
                        {
                            feedback.Flashing_Bool = false;
                            feedback.CurTimerForFlash = 0f;
                            feedback.NumOfTimeflashed = 0;
                            break;
                        }
                        feedback.NumOfTimeflashed++;
                        feedback.CurTimerForFlash = 0f;
                        break;
                    }
                    case false:
                    {
                        if (this.gameObject.GetComponent<SpriteRenderer>().color.a == 1f)
                            this.gameObject.GetComponent<SpriteRenderer>().color = feedback.secondaryColor;
                        feedback.CurTimerForFlash = 0f;
                        break;
                    }
                }
            }
            else
            {
                feedback.CurTimerForFlash += Time.deltaTime;
            }
        }
    }

    void PushBack()
    {
        if (KnockBackBool == true)
        {
            if (KnockBackTimer >= 1f)
            {
                Vector3 Where;
                float Dis = .5f;
                float XDiff = this.gameObject.transform.position.x - KnockBack.x;
                float YDiff = this.gameObject.transform.position.y - KnockBack.y;
                Where = new Vector3(this.gameObject.transform.position.x + XDiff, this.gameObject.transform.position.y + YDiff, this.gameObject.transform.position.z);
                this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, Where, Dis);
                NumKnock++;
                if (NumKnock == MaxNumKnock)
                {
                    KnockBackBool = false;
                    NumKnock = 0;
                }
            }
            else
            {
                KnockBackTimer += Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Rotates the Object to point towards the position passed in
    /// </summary>
    /// <param name="pos">The position vector to rotate to</param>
    private void RotateTo(Vector3 pos)
    {
        Vector3 vectorToTarget = (pos - transform.position).normalized;
        Quaternion lookdir = Quaternion.LookRotation(vectorToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookdir, .1f);
    }

    private void SaveUsables()
    {
        switch (inventory.singleUseObject)
        {
            case singleUseObjects.pebble:
            {
                PlayerPrefs.SetInt("PlayerSUO", 1);
                break;
            }
            case singleUseObjects.healthkit:
            {
                PlayerPrefs.SetInt("PlayerSUO", 2);
                break;
            }
            case singleUseObjects.none:
            {
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion
}