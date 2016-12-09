using UnityEngine;
using System.Collections;

public class PublicSingleton<T> : MonoBehaviour where T : class
{
    public static T instance;

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
