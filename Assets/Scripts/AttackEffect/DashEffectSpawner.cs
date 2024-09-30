using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 大黄蜂冲刺特效生成器
/// </summary>
public class DashEffectSpawner : Spawner
{
    [Header("-冲刺特效")]
    [SerializeField]
    [Tooltip("所属单位")]
    private Enemy belonger;
    [SerializeField]
    [Tooltip("是否为空中冲刺特效")]
    private bool isAirDash;

    protected override GameObject SpawnInstance(GameObject prefab)
    {
        var go = base.SpawnInstance(prefab);
        if(isAirDash)go.transform.localRotation = belonger.transform.localRotation;
        go.transform.localScale = belonger.transform.localScale;
        return go;
    }
}
