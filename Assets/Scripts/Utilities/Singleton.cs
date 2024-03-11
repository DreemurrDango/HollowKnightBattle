using UnityEngine;

/// <summary>
/// 单例模式泛型类：通过继承该类将类变为单例模式
/// </summary>
/// <typeparam name="T">该类类型</typeparam>
public class Singleton<T> : MonoBehaviour where T:Singleton<T>
{
    private static T instance;
    public static T Instance => instance;

    protected virtual void Awake()
    {
        if (instance) Destroy(this);
        else instance = (T)this;
    }

    protected virtual void OnDestroy() { if (instance == this) instance = null; }
}
