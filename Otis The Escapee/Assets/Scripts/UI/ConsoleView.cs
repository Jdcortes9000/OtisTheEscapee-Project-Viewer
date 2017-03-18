using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;
using UnityEngine.EventSystems;

public class ConsoleView : MonoBehaviour
{
    ConsoleController console = new ConsoleController();

    bool didShow = false;

    public GameObject viewContainer; //Container for console view, should be a child of this GameObject
    public Text logTextArea;
    public InputField inputField;

    void Start()
    {
        if (console != null)
        {
            console.visibilityChanged += onVisibilityChanged;
            console.logChanged += onLogChanged;
        }
        updateLogStr(console.log);
    }

    ~ConsoleView()
    {
        console.visibilityChanged -= onVisibilityChanged;
        console.logChanged -= onLogChanged;
    }

    void Update()
    {
        //Toggle visibility when tilde key pressed
        if (Input.GetKeyUp("`"))
        {
            toggleVisibility();
        }

        //Toggle visibility when 5 fingers touch.
        if (Input.touches.Length == 5)
        {
            if (!didShow)
            {
                toggleVisibility();
                didShow = true;
            }
        }
        else
        {
            didShow = false;
        }
    }

    void toggleVisibility()
    {
        setVisibility(!viewContainer.activeSelf);
    }

    void setVisibility(bool visible)
    {
        viewContainer.SetActive(visible);
        if (visible)
        {
            inputField.OnPointerClick(new PointerEventData(EventSystem.current));
            string[] toBePassed = { "inputFalse" };
            EventManager.StringEvent(toBePassed);
        }
        else
        {
            string[] toBePassed = { "inputTrue" };
            EventManager.StringEvent(toBePassed);
        }
    }

    void onVisibilityChanged(bool visible)
    {
        setVisibility(visible);
    }

    void onLogChanged(string[] newLog)
    {
        updateLogStr(newLog);
    }

    void updateLogStr(string[] newLog)
    {
        if (newLog == null)
        {
            logTextArea.text = "";
        }
        else
        {
            logTextArea.text = string.Join("\n", newLog);
        }
    }

    public void runCommand()
    {
        console.runCommandString(inputField.text);
        inputField.text = "";
    }
}