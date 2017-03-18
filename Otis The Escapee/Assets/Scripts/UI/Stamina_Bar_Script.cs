using UnityEngine;
using System.Collections;

public class Stamina_Bar_Script : Bar_Script
{
    public override void ParseStrings(string[] values)
    {
        foreach (string s in values)
        {
            if (s.StartsWith("stamina"))
            {
                string temp = s.Remove(0, 7);
                AmountFilled = float.Parse(temp);
            }
        }
    }
}
