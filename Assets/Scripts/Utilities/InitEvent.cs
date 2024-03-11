using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 暴露各Unity事件，可使用该脚本设置简单的应做动作
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
