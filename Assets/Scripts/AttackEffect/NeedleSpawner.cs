using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 大黄蜂扔针攻击的针攻击物生成器
/// </summary>
public class NeedleSpawner : Spawner
{
    [Header("-针")]
    [SerializeField]
    [Tooltip("所属单位")]
    private Enemy belonger;
    [Tooltip("扔出针的最小初始力")]
    public Vector2 minForce;
    [Tooltip("扔出针的最大初始力")]
    public Vector2 maxForce;
    [Tooltip("针返回至所发出角色时的事件回调")]
    public UnityEvent onNeedleBack;

    protected override GameObject SpawnInstance(GameObject prefab)
    {
        var go = base.SpawnInstance(prefab);
        var needle = go.GetComponent<HornetNeedle>();
        needle.onNeedleBack.AddListener(onNeedleBack.Invoke);
        Vector2 f = Vector2.Lerp(minForce, maxForce, Random.value);
        needle.Init(this,belonger.transform, f * transform.lossyScale.x);
        return go;
    }
}
