using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

namespace CCC.Manager
{
    public class BaseManager : MonoBehaviour
    {
        public class BaseManagerEvent: UnityEvent<BaseManager> { }
        [HideInInspector]
        public BaseManagerEvent onCompleteInit = new BaseManagerEvent();
        [HideInInspector]
        public bool initComplete = false;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public virtual void Init()
        {

        }

        protected void CompleteInit()
        {
            initComplete = true;
            onCompleteInit.Invoke(this);
        }
    }
}
