using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CCC.Utility;

[System.Serializable]
public class History
{
    [System.Serializable]
    public class HistoryDay
    {
        public World world;
        public RequestManager.MailBox mailBox;
        public HistoryDay(World world, RequestManager.MailBox mailBox)
        {
            this.world = world;
            this.mailBox = mailBox;
        }
    }

    private int recordsCountLimit = 3;
    List<HistoryDay> past = new List<HistoryDay>();

    public void RecordDay()
    {
        RequestManager.MailBox mailBoxClone = ObjectCopier.Clone(RequestManager.GetMailBox);
        World worldClone = ObjectCopier.Clone(Universe.World);
        HistoryDay day = new HistoryDay(worldClone, mailBoxClone);

        if (past.Count >= recordsCountLimit)
            past.RemoveAt(0);

        past.Add(day);
    }

    public void LoadPast(int days)
    {
        if (past.Count <= 0)
            throw new System.Exception("Cannot load past because there are no recorded days");

        HistoryDay newDay = past[past.Count - 1];
        past.RemoveAt(past.Count - 1);

        for (int i = 0; i < days; i++)
        {
            if (past.Count <= 0)
                break;
            newDay = past[past.Count - 1];
            past.RemoveAt(past.Count - 1);
        }

        if (newDay != null)
        {
            Universe.instance.SetWorldTo(newDay.world);
            RequestManager.ApplyMailBox(newDay.mailBox);
            RecordDay();
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
