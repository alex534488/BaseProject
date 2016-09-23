using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CCC.Manager
{
    public class MasterManager : MonoBehaviour
    {

        public static MasterManager master;

        List<Object> managers;

        void Awake()
        {
            if (master != null) master = this;
            else Destroy(this);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public Object GetManager<T>() where T : BaseManager
        {
            foreach (Object manager in managers)
            {
                if (manager is T) return manager;
            }
            return null;
        }
    }
}
