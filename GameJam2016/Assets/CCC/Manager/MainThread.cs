using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace CCC.Manager
{
    public class MainThread : BaseManager
    {
        static List<UnityAction> actionList = new List<UnityAction>();
        public override void Init()
        {
            base.Init();
            CompleteInit();
        }

        void Update()
        {
            if(actionList.Count > 0)
            {
                foreach(UnityAction action in actionList)
                {
                    action();
                }
                actionList.Clear();
            }
        }

        static public void AddAction(UnityAction action)
        {
            if (action == null) return;
            actionList.Add(action);
        }
    }
}
