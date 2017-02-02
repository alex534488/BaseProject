using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CCC.Utility;

public class StorylineManager : Singleton<StorylineManager>, INewDay
{
    [System.Serializable]
    public class StorylineSave
    {
        public System.Type classType;
        public object savedData;
        public StoryGraph.SaveState graphSave;
        public StorylineSave(System.Type classType, object savedData, StoryGraph.SaveState graphSave)
        {
            this.graphSave = graphSave;
            this.classType = classType;
            this.savedData = savedData;
        }
    }
    [System.Serializable]
    public class StorylineManagerSave
    {
        public List<StorylineSave> saveList;
        public StorylineManagerSave(int capacity = 1)
        {
            saveList = new List<StorylineSave>(capacity);
        }
    }

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

    public void ArrivalDay()
    {
    }

    T GetStoryline<T>() where T : Storyline
    {
        foreach (Storyline storyline in storylinePrefabs)
        {
            if (storyline is T)
                return storyline as T;
        }
        Debug.LogWarning("Cannot find storyline of type: " + typeof(T).ToString());
        return null;
    }
    Storyline GetStoryline(System.Type classType)
    {
        foreach (Storyline storyline in storylinePrefabs)
        {
            if (storyline.GetType() == classType)
                return storyline;
        }
        Debug.LogWarning("Cannot find storyline of type: " + classType.ToString());
        return null;
    }

    static public StorylineManagerSave GetSaveState()
    {
        StorylineManagerSave save = new StorylineManagerSave(instance.ongoing.Count);
        foreach (Storyline storyline in instance.ongoing)
        {
            save.saveList.Add(new StorylineSave(storyline.GetType(), storyline.GetSavedData(), storyline.storyGraph.GetSaveState()));
        }
        return save;
    }

    static public void ApplySaveState(StorylineManagerSave save)
    {
        Terminate();
        foreach (StorylineSave storylineSave in save.saveList)
        {
            Launch(instance.GetStoryline(storylineSave.classType), storylineSave);
        }
    }

    static public T GetOngoing<T>() where T : Storyline
    {
        return GetOngoing(typeof(T)) as T;
    }

    static public Storyline GetOngoing(System.Type type)
    {
        if (instance == null || instance.ongoing == null)
            return null;
        foreach (Storyline storyline in instance.ongoing)
        {
            if (storyline.GetType() == type)
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

    static private void Launch(Storyline storyline, StorylineSave save = null)
    {
        if (storyline == null)
            return;
        print("launch");
        if (IsOngoing(storyline.GetType()))
        {
            Debug.LogError("Cannot launch " + storyline.name + " storyline because one of the same type is already ongoing.");
            return;
        }

        Storyline obj = Instantiate(storyline.gameObject).GetComponent<Storyline>();

        instance.ongoing.Add(obj);
        if (save == null)
            obj.Init(End, null, null);
        else
            obj.Init(End, save.graphSave, save.savedData);
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
        return GetOngoing(type) != null;
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
