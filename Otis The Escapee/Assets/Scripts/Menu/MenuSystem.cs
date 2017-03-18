using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour
{
    public static float JoystickCooldown = .2f;
    public static float buttonCooldown = .2f;

    public enum SelectableItemType { button, slider, toggle }

    [Serializable]
    public class Button
    {
        [SerializeField]
        private string[] ToBePassedOnUseage;

        [SerializeField]
        private UnityEngine.UI.Selectable Selectable;

        [SerializeField]
        private int NextMenuIndex = -1;

        [SerializeField]
        private SelectableItemType itemType = SelectableItemType.button;

        private Dictionary<string, KeyCode> Keys = new Dictionary<string, KeyCode>();
        private bool sentToggle = false;
        private bool OnClickRan = false;
        private Menu.ToggleCallback callback;
        private Menu.changeIndex toChangeIndex;
        private int index;

        private void OnClick()
        {
            EventManager.StringEvent(ToBePassedOnUseage);
            buttonCooldown = .2f;
            OnClickRan = true;
        }

        private void OnPointerEnter(PointerEventData data)
        {
            toChangeIndex.Invoke(index);
        }

        public void Start(int index, Menu.ToggleCallback updateToggle, Menu.changeIndex ToChangeIndex)
        {
            this.index = index;
            toChangeIndex = ToChangeIndex;
            var trigger = Selectable.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = Selectable.gameObject.AddComponent<EventTrigger>();
            }
            var entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            trigger.triggers.Clear();
            entry.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
            trigger.triggers.Add(entry);
            switch (itemType)
            {
                case SelectableItemType.button:
                    {
                        var button = Selectable as UnityEngine.UI.Button;
                        if (button != null)
                        {
                            button.onClick.AddListener(OnClick);
                        }
                        break;
                    }
                case SelectableItemType.slider:
                    {
                        Keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
                        Keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
                        break;
                    }
                case SelectableItemType.toggle:
                    {
                        updateToggle += ToggleUpdate;
                        callback = updateToggle;
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Select()
        {
            Selectable.Select();
        }

        public int Update()
        {
            if (OnClickRan)
            {
                OnClickRan = false;
                return NextMenuIndex;
            }
            switch (itemType)
            {
                case SelectableItemType.button:
                    {
                        if (Input.GetKeyDown(KeyCode.Return) || (Input.GetAxis("AButton") > 0 && buttonCooldown < 0f))
                        {
                            UnityEngine.UI.Button thisButton = Selectable as UnityEngine.UI.Button;
                            if (thisButton != null)
                                thisButton.OnPointerClick(new PointerEventData(EventSystem.current));
                            EventManager.StringEvent(ToBePassedOnUseage);
                            buttonCooldown = .2f;
                            return NextMenuIndex;
                        }
                        break;
                    }
                case SelectableItemType.slider:
                    {
                        if (Input.GetKeyDown(Keys["Left"]) || (Input.GetAxis("LeftJoystickHorizontal") < 0 && JoystickCooldown < 0f))
                        {
                            var thisButton = Selectable as UnityEngine.UI.Slider;
                            if (thisButton != null)
                                thisButton.value -= .1f;
                            EventManager.StringEvent(ToBePassedOnUseage);
                            JoystickCooldown = .2f;
                        }
                        if (Input.GetKeyDown(Keys["Right"]) || (Input.GetAxis("LeftJoystickHorizontal") > 0 && JoystickCooldown < 0f))
                        {
                            var thisButton = Selectable as UnityEngine.UI.Slider;
                            if (thisButton != null)
                                thisButton.value += .1f;
                            EventManager.StringEvent(ToBePassedOnUseage);
                            JoystickCooldown = .2f;
                        }

                        break;
                    }
                case SelectableItemType.toggle:
                    {
                        if (Input.GetKeyDown(KeyCode.Return) || (Input.GetAxis("AButton") > 0 && buttonCooldown < 0f))
                        {
                            sentToggle = true;
                            callback.Invoke();
                            EventManager.StringEvent(ToBePassedOnUseage);
                            buttonCooldown = .2f;
                        }
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return -1;
        }

        private void ToggleUpdate()
        {
            var toggle = Selectable as Toggle;
            if (!sentToggle)
            {
                if (toggle != null) toggle.isOn = false;
            }
            else
            {
                if (toggle != null) toggle.isOn = true;
            }
            sentToggle = false;
        }

        public void UpdateChildText(Color color)
        {
            Selectable.GetComponentInChildren<Text>().color = color;
        
        }
        public void Wiggle()
        {          
            Selectable.GetComponent<Animator>().enabled = true;
            Selectable.GetComponent<Animator>().Play("NewButton");
        }
    }

    [Serializable]
    public class Menu
    {
        public delegate void ToggleCallback();

        public delegate void changeIndex(int newIndex);

        [SerializeField]
        private string[] ToBePassedOnExit;

        [SerializeField]
        private Button[] Buttons;

        [SerializeField]
        private int initialIndex;

        private int index = 0;
        private MenuSystem systemPointer;

        private ToggleCallback UpdateToggles;

        private changeIndex forButtons;

        public Button getCurrentButton()
        {
            return Buttons[index];
        }

        public void Start(MenuSystem pointer)
        {   
            forButtons = new changeIndex(ChangeIndex);
            index = initialIndex;
            Buttons[index].Select();
            for (int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i].Start(i, UpdateToggles, forButtons);
            }
            UpdateVisuals();
            systemPointer = pointer;
        }

        public void ChangeIndex(bool Up = false)
        {
            if (Up)
            {
                index--;
                if (index < 0)
                {
                    index = Buttons.Length - 1;
                }
                Buttons[index].Select();
            }
            else
            {
                index++;
                if (index > Buttons.Length - 1)
                {
                    index = 0;
                }
                Buttons[index].Select();
            }
            UpdateVisuals();
        }

        private void ChangeIndex(int newIndex)
        {
            index = newIndex;
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            for (int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i].UpdateChildText(Color.black);
            }
            Buttons[index].UpdateChildText(Color.yellow);
            Buttons[index].Wiggle();
        }

        public void Update()
        {
            int x = Buttons[index].Update();
            switch (x)
            {
                case -1:
                    {
                        break;
                    }
                case -2:
                    {
                        systemPointer.ReturnToPrevMenu();
                        break;
                    }
                default:
                    {
                        systemPointer.ChangeMenu(x);
                        break;
                    }
            }
        }

        public void OnExit()
        {
            EventManager.StringEvent(ToBePassedOnExit);
        }
    }

    [SerializeField]
    private string[] ToBePassedOnExit;

    [SerializeField]
    private string unpauseInputEvent;

    [SerializeField]
    private string pauseInputEvent;

    [SerializeField]
    private Menu[] Menus;

    [SerializeField]
    private int initialIndex;

    [SerializeField]
    private Button currentButton;

    private int MenuIndex;
    private Stack<int> PrevIdexes = new Stack<int>();
    private Dictionary<string, KeyCode> Keys = new Dictionary<string, KeyCode>();

    [SerializeField]
    private bool pause = true;

    [SerializeField]
    private bool upDown = true;

    [SerializeField]
    private bool invertControl = false;

    private bool canUnpause = false;

    private void Start()
    {
        if (upDown)
        {
            if (invertControl)
            {
                Keys.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W")));
                Keys.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
            }
            else
            {
                Keys.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W")));
                Keys.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
            }
        }
        else
        {
            if (invertControl)
            {
                Keys.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
                Keys.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
            }
            else
            {
                Keys.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
                Keys.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
            }
        }
        Menus[MenuIndex].Start(this);
        EventManager.PassStrings += ParseStrings;
    }

    private void OnDestroy()
    {
        EventManager.PassStrings -= ParseStrings;
    }

    private void Update()
    {
        buttonCooldown -= Time.deltaTime;
        JoystickCooldown -= Time.deltaTime;
        currentButton = Menus[MenuIndex].getCurrentButton();
        if (pause)
        {
            ResetMenu();
            return;
        }
        if (Input.GetKeyDown(Keys["Up"]) || (-Input.GetAxis("LeftJoystickVertical") > 0 && JoystickCooldown < 0f))
        {
            Menus[MenuIndex].ChangeIndex(true);
            JoystickCooldown = .2f;
        }
        else if (Input.GetKeyDown(Keys["Down"]) || (-Input.GetAxis("LeftJoystickVertical") < 0 && JoystickCooldown < 0f))
        {
            Menus[MenuIndex].ChangeIndex();
            JoystickCooldown = .2f;
        }
        if ((Input.GetKeyDown(KeyCode.Escape) || (Input.GetAxis("BButton") > 0 && buttonCooldown < 0f)) && canUnpause)
        {
            ReturnToPrevMenu();
            buttonCooldown = .2f;
            canUnpause = false;
        }
        else
        {
            canUnpause = true;
        }
        Menus[MenuIndex].Update();
    }

    private void ResetMenu()
    {
        MenuIndex = initialIndex;
        Menus[MenuIndex].Start(this);
    }

    public void ChangeMenu(int newIndex)
    {
        PrevIdexes.Push(MenuIndex);
        MenuIndex = newIndex;
        Menus[MenuIndex].Start(this);
    }

    public void ReturnToPrevMenu()
    {
        if (PrevIdexes.Count < 1)
            EventManager.StringEvent(ToBePassedOnExit);
        else
        {
            Menus[MenuIndex].OnExit();
            MenuIndex = PrevIdexes.Pop();
        }
    }

    private void ParseStrings(string[] values)
    {
        foreach (string s in values)
        {
            if (s == pauseInputEvent)
            {
                pause = true;
            }
            else if (s == unpauseInputEvent)
            {
                pause = false;
            }
        }
    }
}