using System;
using Constant;
using DataCollection;
using UnityEngine;

public static class EventHandler
{
    /// <summary>
    /// �¼�����
    /// </summary>
    public static event Action<int> OnEvent;
    public static void CallEvent(int arg) =>
        OnEvent?.Invoke(arg);
}
