using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : class
{
    protected static T instance;

    void Awake()
    {
        if (!(this is T)) return;

        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
