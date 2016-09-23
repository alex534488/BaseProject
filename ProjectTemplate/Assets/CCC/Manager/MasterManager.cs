using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CCC.Manager
{
    public class MasterManager : MonoBehaviour
    {

        public static MasterManager master;

        /// <summary>
        /// This only contains the prefab. It does not point towards any actual manager.
        /// </summary>
        public List<BaseManager> managersPrefab;
        public List<BaseManager> managers;

        void Awake()
        {
            if (master != null) master = this;
            else Destroy(this);

            foreach(BaseManager managerPrefab in managersPrefab)
            {
                BaseManager actualManager = GameObject.Instantiate(managerPrefab).GetComponent<BaseManager>();
                managers.Add(actualManager);
            }
        }

        public T GetManager<T>() where T : BaseManager
        {
            foreach (BaseManager manager in managers)
            {
                if (manager is T) return manager as T;
            }
            return null;
        }
    }
}
