using UnityEngine;
using System.Collections;

public class Resume : MonoBehaviour
{
    public void UnpauseGame()
    {
        string[] toBePassed = {"unpause"};
        EventManager.StringEvent(toBePassed);
    }
}