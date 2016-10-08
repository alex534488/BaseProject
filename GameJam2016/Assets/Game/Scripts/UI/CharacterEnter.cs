using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class CharacterEnter : MonoBehaviour {
    public Transform spawn;
    public Transform[] moveToEnter;
    public Transform[] moveToExit;
    public Character characterPrefab;

    static CharacterEnter main;
    static Character currentCharacter;
    static UnityAction onEnterComplete;
    static UnityAction onExitComplete;

    void Awake()
    {
        if (main == null) main = this;
    }

    public static void Enter(UnityAction onEnterComplete = null)
    {
        if(currentCharacter != null)
        {
            Debug.LogError("Cannot enter another character in the court !");
            return;
        }
        CharacterEnter.onEnterComplete = onEnterComplete;

        currentCharacter = Instantiate(main.characterPrefab.gameObject).GetComponent<Character>();

        currentCharacter.Init(main.spawn);

        //Queue destinations
        foreach(Transform tr in main.moveToEnter)
        {
            currentCharacter.QueueMoveTo(tr, OnEnterComplete);
        }
    }

    public static void Exit(UnityAction onExitComplete = null)
    {
        if (currentCharacter == null)
        {
            Debug.LogError("Cannot exit a character when none are in the court !");
            return;
        }
        CharacterEnter.onExitComplete = onExitComplete;
        foreach (Transform tr in main.moveToExit)
        {
            currentCharacter.QueueMoveTo(tr, OnExitComplete);
        }
    }

    static void OnEnterComplete()
    {
        if (onEnterComplete != null) onEnterComplete.Invoke();
    }

    static void OnExitComplete()
    {
        Destroy(currentCharacter.gameObject);
        currentCharacter = null;
        if (onExitComplete != null) onExitComplete.Invoke();
    }
}
