using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fred.Manager
{
    public class BaseManager : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
