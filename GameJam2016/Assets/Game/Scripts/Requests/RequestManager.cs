using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public enum Priority
{
    Urgent = 1,
    Standard = 2,
    Mundane = 3
}

public class RequestManager : Singleton<RequestManager>
{
    [System.Serializable]
    public class RequestPacket
    {
        public Request request;
        public int delay;
        public Priority priority;
        public RequestPacket(Request request, int delay = 0, Priority priority = Priority.Standard)
        {
            this.request = request;
            this.delay = delay;
            this.priority = priority;
        }
        public bool IsReady() { return delay <= 0; }
        public void NewDay() { delay--; }
    }
    [System.Serializable]
    public class MailBox
    {
        public List<RequestPacket> delayedMail = new List<RequestPacket>();
        public List<RequestPacket> pendingMail = new List<RequestPacket>();
        public void NewDay()
        {
            for (int i = 0; i < delayedMail.Count; i++)
            {
                delayedMail[i].NewDay();
                if (delayedMail[i].IsReady())
                {
                    pendingMail.Add(delayedMail[i]);
                    delayedMail.RemoveAt(i);
                    i--;
                }
            }
        }
        public bool HasPendingMail() {  return pendingMail.Count > 0;  }
        public void AddMail(RequestPacket packet)
        {
            if (packet.delay > 0)
                delayedMail.Add(packet);
            else
                pendingMail.Add(packet);
        }
    }

    public RequestBank bank;
    private MailBox mailBox = new MailBox();

    public bool isRequesting = false;
    static public bool IsRequesting { get { return instance.isRequesting; } }

    public UnityEvent onCompleteRequests = new UnityEvent();
    static public UnityEvent OnCompleteRequests { get { return instance.onCompleteRequests; } }

    public UnityEvent onBeginRequests = new UnityEvent();
    static public UnityEvent OnBeginRequests { get { return instance.onBeginRequests; } }

    public void NewDay()
    {
        mailBox.NewDay();

        //Si ya du mail
        if (mailBox.HasPendingMail())
        {
            //et qu'on est pas déjà entrain de request
            if(!isRequesting) 
                ExecuteNextRequest();
        }
    }

    public static void ExecuteNextRequest()
    {
        if (instance.mailBox.pendingMail.Count <= 0)
        {
            Debug.LogWarning("Cannot execute next request. The request list is empty");
            return;
        }

        bool wasInRequest = instance.isRequesting;
        instance.isRequesting = true;

        if (!wasInRequest)
        {
            print("begin requests");
            instance.onBeginRequests.Invoke();
        }

        instance.isRequesting = true;
        instance.mailBox.pendingMail[0].request.DoRequest();
    }

    public static void OnRequestComplete()
    {
        //Remove request from list
        instance.mailBox.pendingMail.RemoveAt(0);

        //Do next request ?

        //Mail box empty
        if (instance.mailBox.HasPendingMail())
        {
            ExecuteNextRequest();
        }
        //Mail box not empty
        else
        {
            instance.isRequesting = false;
            instance.onCompleteRequests.Invoke();
            print("completed all requests for today");
        }
    }

    public void ArrivalDay()
    {
        // TODO: Faire la premier requete du jeu
    }

    public static void SendRequest(Request request, int delay = 0, Priority priority = Priority.Standard)
    {
        instance.mailBox.AddMail(new RequestPacket(request, delay, priority));

        if (!instance.isRequesting && instance.mailBox.pendingMail.Count > 0)
            ExecuteNextRequest();
    }

    public static void DeleteAllRequests()
    {
        instance.mailBox.pendingMail.Clear();
    }

    //Request bank methods

    public static RequestFrame GetRequestFrame(string tag)
    {
        if (instance == null) { Debug.LogError("Error: Request manager instance is null."); return null; }
        if (instance.bank == null) { Debug.LogError("Error: Request manager's bank is null."); return null; }

        return instance.bank.GetFrame(tag);
    }

    public static bool BuildAndSendRequest(string tag, Village source, Village destination, int value = 1, ResourceType type = ResourceType.custom,
        Command[] commands = null, int delay = 0, Priority priority = Priority.Standard)
    {
        RequestFrame frame = GetRequestFrame(tag);
        if (frame == null) return false;

        SendRequest(frame.Build(source, destination, value, type, commands), delay, priority);
        return true;
    }

}

