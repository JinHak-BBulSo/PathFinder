using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance = default;
    
    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<T>();
            DontDestroyOnLoad(gameObject);
        }
    }
}
