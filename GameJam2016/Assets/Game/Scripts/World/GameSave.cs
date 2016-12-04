using UnityEngine;
using System.Collections;

public class GameSave
{
    static public string GetFilePath()
    {
        return Application.persistentDataPath + "/save1_";
    }
}