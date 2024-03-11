using System;
using Constant;
using DataCollection;
using UnityEngine;

public static class EventHandler
{
    /// <summary>
    /// 事件样本
    /// </summary>
    public static event Action<int> OnEvent;
    public static void CallEvent(int arg) =>
        OnEvent?.Invoke(arg);
}
