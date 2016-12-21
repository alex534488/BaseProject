﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CCC.Utility;

public class StorylineManager : Singleton<StorylineManager>, INewDay
{

    public List<Storyline> storylinePrefabs = new List<Storyline>();
    List<Storyline> ongoing = new List<Storyline>();

    public void NewDay()
    {
        //On fait une nouvelle liste, parce que si certaine d'entre elle se termine,
        //elle seront retiré de la liste des 'ongoing'. On ne veut pas brisé le foreach loop
        List<Storyline> todo = new List<Storyline>(ongoing);
        foreach (Storyline storyline in todo)
        {
            if (!storyline.IsComplete)
                storyline.NewDay();
        }
    }

    Storyline GetStoryline<T>() where T : Storyline
    {
        foreach (Storyline storyline in storylinePrefabs)
        {
            if (storyline is T)
                return storyline;
        }
        return null;
    }

    static private void End(Storyline storyline)
    {
        Terminate(storyline);
    }

    static private Storyline LotteryByTag(string tag)
    {
        Lottery lottery = new Lottery();
        foreach (Storyline storyline in instance.storylinePrefabs)
        {
            string storyTag = storyline.Tag;

            if (storyTag.Length > tag.Length)                   //Si le tag est plus long que celui demandé (ex: village_random_gold > village_random)
                storyTag = storyTag.Substring(0, tag.Length);   //on va le couper pour seulement tenir compte de la première partie (village_random)

            if (storyTag == tag && !IsOngoing(storyline.GetType()))
                lottery.Add(storyline, 1);
        }
        if (lottery.Count > 0)
            return lottery.Pick() as Storyline;
        else
            return null;
    }

    /// <summary>
    /// Launch one of the specified storyline (according to tag)
    /// </summary>
    static public void Launch(string tag)
    {
        Launch(LotteryByTag(tag));
    }

    /// <summary>
    /// Launch the specified storyline
    /// </summary>
    static public void Launch<T>() where T : Storyline
    {
        Launch(instance.GetStoryline<T>());
    }

    static private void Launch(Storyline storyline)
    {
        if (storyline == null)
            return;
        if (IsOngoing(storyline.GetType()))
        {
            Debug.LogError("Cannot launch " + storyline.name + " storyline because one of the same type is already ongoing.");
            return;
        }

        Storyline obj = Instantiate(storyline.gameObject).GetComponent<Storyline>();

        instance.ongoing.Add(obj);
        obj.Init(End);
    }

    /// <summary>
    /// Ends all ongoing storylines
    /// </summary>
    static public void Terminate()
    {
        int count = instance.ongoing.Count;
        for (int i = 0; i < count; i++)
        {
            Terminate(instance.ongoing[0]);
        }
    }

    /// <summary>
    /// Ends the specified ongoing storylines (can be more than one, according to tag)
    /// </summary>
    static public void Terminate(string tag)
    {
        List<Storyline> terminateList = new List<Storyline>();
        foreach (Storyline storyline in instance.ongoing)
        {
            string storyTag = storyline.Tag;

            if (storyTag.Length > tag.Length)                   //Si le tag est plus long que celui demandé (ex: village_random_gold > village_random)
                storyTag = storyTag.Substring(0, tag.Length);   //on va le couper pour seulement tenir compte de la première partie (village_random)

            if (storyTag == tag)
                terminateList.Add(storyline);
        }

        for (int i = 0; i < terminateList.Count; i++)
        {
            Terminate(terminateList[i]);
        }
    }

    static public void Terminate<T>() where T : Storyline
    {
        foreach (Storyline storyline in instance.ongoing)
        {
            if(storyline is T)
            {
                Terminate(storyline);
                return;
            }
        }
        Debug.LogError("Cannot terminate " + typeof(T).Name + " storyline because it is not ongoing");
    }

    /// <summary>
    /// Ends the specified ongoing storyline
    /// </summary>
    static private void Terminate(Storyline storyline)
    {
        if (storyline == null)
            return;
        if (!Ongoing.Contains(storyline))
        {
            Debug.LogError("Cannot terminate " + storyline.name + " storyline because it is not ongoing");
            return;
        }
        storyline.Terminate();
        instance.ongoing.Remove(storyline);
        Destroy(storyline.gameObject);
    }

    /// <summary>
    /// List of ongoing storyline
    /// </summary>
    static public List<Storyline> Ongoing
    {
        get
        {
            return instance == null ? null : instance.ongoing;
        }
    }

    /// <summary>
    /// The amount of ongoing storylines
    /// </summary>
    static public int OngoingCount
    {
        get
        {
            return Ongoing.Count;
        }
    }

    /// <summary>
    /// Is any storyline ongoing ?
    /// </summary>
    static public bool IsOngoing()
    {
        return instance == null || instance.ongoing == null ? false : instance.ongoing.Count > 0;
    }

    /// <summary>
    /// Is the specified storyline ongoing ?
    /// </summary>
    static public bool IsOngoing<T>() where T : Storyline
    {
        return IsOngoing(typeof(T));
    }

    static bool IsOngoing(System.Type type)
    {
        if (instance == null || instance.ongoing == null)
            return false;
        foreach (Storyline storyline in instance.ongoing)
        {
            if (storyline.GetType() == type)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Is one of the specifieds storyline ongoing ?
    /// </summary>
    static public bool IsOngoing(string tag)
    {
        if (instance == null || instance.ongoing == null)
            return false;
        foreach (Storyline storyline in instance.ongoing)
        {
            string storyTag = storyline.Tag;

            if (storyTag == "")
                continue;

            if (storyTag.Length > tag.Length)                   //Si le tag est plus long que celui demandé (ex: village_random_gold > village_random)
                storyTag = storyTag.Substring(0, tag.Length);   //on va le couper pour seulement tenir compte de la première partie (village_random)

            if (storyTag == tag)
                return true;
        }
        return false;
    }
}
