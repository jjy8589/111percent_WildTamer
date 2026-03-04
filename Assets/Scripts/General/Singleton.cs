using UnityEngine;
using System;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool _applicationIsQuitting = false;
    private static T _instance;
    private static readonly object _lock = new object();

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.LogWarning($"[Singleton] {typeof(T)} already destroyed on application quit.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        // Scene에 생성된 Singleton이 없다면 자동 생성
                        var singletonObj = new GameObject($"{typeof(T)} (Singleton)");
                        _instance = singletonObj.AddComponent<T>();

                        Debug.Log($"[Singleton] Created new instance of {typeof(T)}");
                    }

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError($"[Singleton] Something went really wrong - there should never be more than 1 singleton! Reopening the scene might fix it.");
                    }
                    else if (FindObjectsOfType(typeof(T)).Length == 1)
                    {
                        Debug.Log($"[Singleton] Using existing instance of {typeof(T)}: {_instance.gameObject.name}");
                    }
                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Debug.LogWarning($"[Singleton] Duplicate {typeof(T)} found. Destroying {gameObject.name}.");
            Destroy(gameObject);
        }
    }

    protected virtual void OnApplicationQuit()
    {
        _applicationIsQuitting = true;
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _applicationIsQuitting = true;
            _instance = null;
        }
    }
}