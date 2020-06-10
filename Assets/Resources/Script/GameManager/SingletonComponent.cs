using UnityEngine;

public class SingletonComponent<T> : MonoBehaviour where T : SingletonComponent<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                T obj = FindObjectOfType<T>();
                if(obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
            }
            return instance;
        }
    }
    
    protected virtual void Awake()
    {
        T[] objs = FindObjectsOfType<T>();
        if (objs.Length > 1)
            Destroy(this);

        DontDestroyOnLoad(this);
    }

}
