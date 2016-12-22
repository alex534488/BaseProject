using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : class
{
    protected static T instance;

    void Awake()
    {
        if (!(this is T))
        {
            Debug.LogError("Trying to make a Singleton<" + typeof(T).Name + "> but instance is a " + this.GetType().Name + ".");
            return;
        }

        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        if ((object)instance == this)
            instance = null;
    }
}
