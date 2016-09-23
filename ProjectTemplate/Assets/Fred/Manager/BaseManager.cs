using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CCC.Manager
{
    public class BaseManager : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
