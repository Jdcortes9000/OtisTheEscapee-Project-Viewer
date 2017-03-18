using UnityEngine;

using System;
using System.Collections.Generic;
using System.Text;
//using UnityEditor.VersionControl;
using UnityEngine.SceneManagement;

public class ConsoleController
{

    #region Event declarations
    // Used to communicate with ConsoleView
    public delegate void LogChangedHandler(string[] log);
    public event LogChangedHandler logChanged;

    public delegate void VisibilityChangedHandler(bool visible);
    public event VisibilityChangedHandler visibilityChanged;
    #endregion

    /// <summary>
    /// Object to hold information about each command
    /// </summary>
    class helpRegistration
    {
        public string command { get; private set; }
        public string syntax { get; private set; }
        public string help { get; private set; }
        public int numReqArgs { get; private set; }

        public helpRegistration(string command, string syntax, string help, int reqArgs)
        {
            this.command = command;
            this.syntax = syntax;
            this.help = help;
            numReqArgs = reqArgs;
        }
    }

    /// <summary>
    /// How many log lines should be retained?
    /// Note that strings submitted to appendLogLine with embedded newlines will be counted as a single line.
    /// </summary>
    const int scrollbackSize = 20;

    Queue<string> scrollback = new Queue<string>(scrollbackSize);
    Dictionary<string, helpRegistration> help = new Dictionary<string, helpRegistration>();

    public string[] log { get; private set; } //Copy of scrollback as an array for easier use by ConsoleView


    public ConsoleController()
    {
        registerHelp("help", "help <second command>", "Returns a list of all commands and their syntax. With a second command will give specific information about the use of said command", 0);
        registerHelp("VColor", "VColor [float (r)] [float (g)] [float (b)] [float (a)]","Changes the color of the vignette over a short period of time, colors must be given between 0 and 1 without a postfix", 4);
        registerHelp("InstVColor", "InstVColor [float (r)] [float (g)] float (b)] [float (a)]","Changes the color of the vignette instantly, colors must be given between 0 and 1 without a postfix", 4);
        registerHelp("VPower", "VPower [float (power)]", "changes the power of the vignette over time, value between 0 and 6", 1);
        registerHelp("InstVPower", "InstVPower [float (power)]", "changes the power of the vignette instantly, value between 0 and 6", 1);
        registerHelp("GSPower", "GSPower [float (power)]", "Changes the power of the greyscale over time, value between 0 and 1", 1);
        registerHelp("InstGSPower", "InstGSPower [float (power)]", "Changes the power of the greyscale instantly, value between 0 and 1", 1);
        registerHelp("giveHatchet", "giveHatchet", "Gives the player a hatchet", 0);
        registerHelp("takeHatchet", "takeHatchet", "Takes away the player's hatchet", 0);
        registerHelp("giveKey", "giveKey", "Gives the player a key", 0);
        registerHelp("takeKey", "takeKey", "Takes the player's key away", 0);
        registerHelp("inputFalse", "inputFalse", "Disables player input until console window is closed", 0);
        registerHelp("inputTrue", "inputTrue", "Enables player input until console window is open", 0);
        registerHelp("PSpeed", "PSpeed [float (speed)]", "Changes the player's base walking speed", 1);
        registerHelp("save", "save [int (file number)]", "Saves a file onto the file number", 1);
        registerHelp("Load", "Load [int (file number)]", "Loads a file from the file number", 1);
        registerHelp("camShake", "camShake [float (length of time)] [float (strength)] [float (axis x)] [float (axis y)] [float (axis z)]", "Shakes the camera for the specified amount of time based on the strength of shaking along the specified axis", 2);
        registerHelp("pause", "pause", "Pauses the game", 0);
        registerHelp("unpause", "unpause", "Unpauses the game", 0);
    }

    void registerHelp(string command, string syntax, string help, int numReqArgs)
    {
        this.help.Add(command, new helpRegistration(command, syntax, help, numReqArgs));
    }

    public void appendLogLine(string line)
    {
        Debug.Log(line);

        if (scrollback.Count >= scrollbackSize)
        {
            scrollback.Dequeue();
        }
        scrollback.Enqueue(line);

        log = scrollback.ToArray();
        if (logChanged != null)
        {
            logChanged(log);
        }
    }

    public void runCommandString(string commandString)
    {
        string chosenCommand = string.Empty;
        foreach (var s in help)
        {
            if (!commandString.StartsWith(s.Key)) continue;
            chosenCommand = s.Key;
            break;
        }
        if (chosenCommand == "help")
        {
            string[] argStrings = commandString.Split(' ');
            if (argStrings.Length >= 2)
            {
                appendLogLine(help[argStrings[1]].help);
            }
            else
            {
                StringBuilder newString = new StringBuilder();
                foreach (var s in help)
                {
                    newString.Append(s.Value.command + " - " + s.Value.syntax + '\n');
                }
                appendLogLine(newString.ToString());
            }
        }
        else if (chosenCommand != string.Empty)
        {
            var commandData = help[chosenCommand];
            string[] argStrings = commandString.Split(' ');
            if (argStrings.Length - 1 >= commandData.numReqArgs)
            {
                StringBuilder comandString = new StringBuilder(commandData.command);
                for (int i = 1; i < argStrings.Length; i++)
                {
                    if (i == 1)
                    {
                        comandString.Append(argStrings[i]);
                    }
                    else
                    {
                        comandString.Append(',' + argStrings[i]);
                    }
                }
                string[] toBePassed = {comandString.ToString()};
                EventManager.StringEvent(toBePassed);
                appendLogLine("Command Successful! " + toBePassed[0]);
            }
            else appendLogLine("Could not run command: too few arguments");
        }
        else
        {
            appendLogLine("Could not find command");
        }
    }
}