using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Fred.Utility
{
    public class ActionQueue : MonoBehaviour
    {
        public class ActionEvent: UnityEvent<Action> { }

        public struct Action
        {
            public UnityAction action;
            public Transform target;
        }

        List<Action> list;
        public UnityEvent onQueueCompleted;
        public ActionEvent onNextItem = new ActionEvent();
        public bool AutoPlay = true;

        private bool isPlaying = false;
        public bool IsPlaying
        {
            get
            {
                return isPlaying;
            }
            private set
            {
                isPlaying = value;
            }
        }

        void Awake()
        {
            list = new List<Action>();
        }

        public void Prioritize(UnityAction action)
        {
            int moved = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].action == action)
                {
                    if (i != 0)
                    {
                        Action priority = list[i];
                        list[i] = list[moved];
                        list[moved] = priority;
                    }
                    moved++;
                }
            }
        }

        public void Add(UnityAction action, Transform target = null)
        {
            Action act = new Action();
            act.action = action;
            act.target = target;

            list.Add(act);

            if (AutoPlay && list.Count == 1) NextItem();
        }

        public void EndItem()
        {
            if (list.Count <= 0) return;

            list.RemoveAt(0);

            if (list.Count > 0) NextItem();
            else
            {
                isPlaying = false;
                onQueueCompleted.Invoke();
            }
        }

        public int Count()
        {
            return list.Count;
        }

        public void Play()
        {
            if (list.Count > 0 && !IsPlaying) NextItem();
        }

        private void NextItem()
        {
            IsPlaying = true;
            list[0].action.Invoke();

            onNextItem.Invoke(list[0]);
        }
    }
}
