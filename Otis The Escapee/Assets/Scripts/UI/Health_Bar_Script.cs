using UnityEngine;
using System.Collections;

public class Health_Bar_Script : Bar_Script
{
    public override void ParseStrings(string[] values)
    {
        foreach (string s in values)
        {
            if (s.StartsWith("health"))
            {
                string temp = s.Remove(0, 6);
                AmountFilled = float.Parse(temp);
                string[] toBePassed = {string.Format("GSPower{0}", 1 - AmountFilled / 200)};
                EventManager.StringEvent(toBePassed);
            }
        }
    }
}
