using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

[System.Serializable]
public enum CommandType
{
    ProgressStoryline = 0,
    Print = 1
}
[System.Serializable]
public class Command
{
    CommandType type;
    string[] parameters = null;
    public Command(CommandType type, params string[] parameters)
    {
        this.type = type;
        this.parameters = parameters;
    }
    /// <summary>
    /// Note: Choice = de 0 à 2
    /// </summary>
    static public Command ProgressStoryline(string className, string choice)
    {
        return new Command(CommandType.ProgressStoryline, className, choice);
    }

    public void Execute()
    {
        switch (type)
        {
            default:
                break;
            case CommandType.ProgressStoryline:
                StorylineManager.GetOngoing(Type.GetType(parameters[0])).Progress(Convert.ToInt32(parameters[1]));
                break;
            case CommandType.Print:
                Debug.Log(parameters[0]);
                break;
        }
    }
}
