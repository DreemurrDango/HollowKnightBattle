using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��¶��Unity�¼�����ʹ�øýű����ü򵥵�Ӧ������
/// </summary>
public class InitEvent : MonoBehaviour
{
    public UnityEvent onAwake;
    public UnityEvent onEnable;
    public UnityEvent onStart;
    public UnityEvent onDisable;
    public UnityEvent onDestroy;

    private void Awake() => onAwake?.Invoke();
    private void OnEnable() => onEnable?.Invoke();
    private void Start() => onStart?.Invoke();
    private void OnDisable() => onDisable?.Invoke();
    private void OnDestroy() => onDestroy?.Invoke();
}
