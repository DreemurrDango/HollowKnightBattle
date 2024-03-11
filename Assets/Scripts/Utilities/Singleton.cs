using UnityEngine;

/// <summary>
/// ����ģʽ�����ࣺͨ���̳и��ཫ���Ϊ����ģʽ
/// </summary>
/// <typeparam name="T">��������</typeparam>
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
