using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace CCC.Manager
{
    public class DelayManager : BaseManager<DelayManager>
    {
        static public void CallTo(UnityAction action, float delay, bool realTime = true)
        {
            if (instance == null)
            {
                Debug.LogError("Tried to call a delay, but the manager is null. Was it properly loaded by MasterManager ?");
                return;
            }
            instance.InstanceCallTo(action, delay, realTime);
        }

        protected override void Awake()
        {
            base.Awake();
            instance = this;
        }

        public override void Init()
        {
            CompleteInit();
        }

        void InstanceCallTo(UnityAction action, float delay, bool realTime = true)
        {
            StartCoroutine(DelayedCallTo(action, delay, realTime));
        }

        IEnumerator DelayedCallTo(UnityAction action, float delay, bool realTime = true)
        {
            if (realTime) yield return new WaitForSecondsRealtime(delay);
            else yield return new WaitForSeconds(delay);

            action.Invoke();
        }
    }
}
