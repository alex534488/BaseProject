using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace CCC.Manager
{
    public class MasterManager : MonoBehaviour
    {
        public static MasterManager master;
        public static bool initComplete = false;

        /// <summary>
        /// This only contains the prefab. It does not point towards any actual manager.
        /// </summary>
        public List<BaseManager> managersPrefab;
        [HideInInspector]
        public List<BaseManager> managers;

        static List<System.Action> onAllInitComplete = new List<System.Action>();

        int inLoadingManagers = 0;

        static public void Sync(System.Action initCallback = null)
        {
            CheckInstance();

            if(initCallback != null)
            {
                if (initComplete) initCallback();
                else onAllInitComplete.Add(initCallback);
            }
        }

        void Awake()
        {
            if (master == null) master = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            DOTween.Init();
            initComplete = false;

            foreach (BaseManager managerPrefab in managersPrefab)
            {
                BaseManager actualManager = GameObject.Instantiate(managerPrefab).GetComponent<BaseManager>();
                managers.Add(actualManager);
                if (!actualManager.initComplete)
                {
                    actualManager.onCompleteInit.AddListener(OnCompleteManagerInit);
                    inLoadingManagers++;
                }
                actualManager.Init();
            }
            CheckCompletion();
        }

        public T GetManager<T>() where T : BaseManager
        {
            if(!initComplete) Debug.LogError("Trying to get a manager while some are still loading! Use 'Sync(System.Action)' to make sure you act after all managers are loaded.");

            foreach (BaseManager manager in managers)
            {
                if (manager is T) return manager as T;
            }
            return null;
        }

        void OnCompleteManagerInit(BaseManager manager)
        {
            manager.onCompleteInit.RemoveListener(OnCompleteManagerInit);
            inLoadingManagers--;
            CheckCompletion();
        }

        /// <summary>
        /// Checks if all managers are done with their Init(). Executes all 'sync' actions if true.
        /// </summary>
        void CheckCompletion()
        {
            if (initComplete) return;

            if (inLoadingManagers <= 0)
            {
                foreach (System.Action action in onAllInitComplete)
                {
                    action();
                    onAllInitComplete.Remove(action);
                }
                initComplete = true;
            }
        }

        /// <summary>
        /// Checks if there is a MasterManager instance. If not, spawn one from the \Resources folder
        /// </summary>
        static void CheckInstance()
        {
            if (master != null) return;

            GameObject.Instantiate(Resources.Load<GameObject>("MasterManager"));
        }
    }
}
