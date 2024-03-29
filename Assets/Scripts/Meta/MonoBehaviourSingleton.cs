﻿using UnityEngine;

/// <summary>
/// Inherit this to create a Singleton which you can access with its "Instance" property.
/// example: public class MyAudioController : GenericSingletonClass<MyAudioController>{}
/// </summary>
public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T> ();
                if (instance == null)
                {
                    GameObject obj = new GameObject ();
                    obj.name = typeof (T).Name;
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    instance = obj.AddComponent<T> ();
                }
            }
            return instance;
        }
    }

    public virtual void Awake ()
    {
        if (instance == null)
        {
            instance = this as T;

            // If this is a root object, don't destroy it on load
            if (this.transform.parent == null) DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy (gameObject);
        }
    }
}