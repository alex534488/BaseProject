using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CCC.Utility;
using UnityEngine.Events;
using System.Runtime.Serialization;

[System.Serializable]
public class History
{
    [System.Serializable]
    public class HistoryDay
    {
        public World world;
        public RequestManager.MailBox mailBox;
        public StorylineManager.StorylineManagerSave storylinesSave;
        public HistoryDay(World world, RequestManager.MailBox mailBox, StorylineManager.StorylineManagerSave storylinesSave)
        {
            this.world = world;
            this.mailBox = mailBox;
            this.storylinesSave = storylinesSave;
        }
    }

    private int recordsCountLimit = 5;
    List<HistoryDay> past = new List<HistoryDay>();
    [System.NonSerialized]
    private UnityEvent onPastLoaded = new UnityEvent();
    public UnityEvent OnPastLoaded { get { return onPastLoaded; } }

    [OnDeserialized]
    public void OnLoad(StreamingContext context)
    {
        onPastLoaded = new UnityEvent();
    }

    public void RecordDay()
    {
        HistoryDay day = ObjectCopier.Clone(new HistoryDay(Universe.World, RequestManager.GetMailBox, StorylineManager.GetSaveState()));

        if (past.Count >= recordsCountLimit)
            past.RemoveAt(0);

        past.Add(day);
    }

    public void LoadPast(int days)
    {
        if (past.Count <= 0)
            throw new System.Exception("Cannot load past because there are no recorded days");

        HistoryDay newDay = past[past.Count - 1];

        for (int i = 0; i < days; i++)
        {
            if (past.Count <= 0)
                break;
            past.RemoveAt(past.Count - 1);
            newDay = past[past.Count - 1];
        }

        if (newDay != null)
        {
            Universe.instance.SetWorldTo(newDay.world);
            RequestManager.ApplyMailBox(newDay.mailBox);
            StorylineManager.ApplySaveState(newDay.storylinesSave);
            onPastLoaded.Invoke();
        }
        else
            Debug.LogError("Error loading past");
    }

    public World ViewPastWorld(int days)
    {
        if (days >= past.Count || days < 0)
            return null;

        int index = (past.Count - 1) - days;

        return past[index].world;
    }
}
