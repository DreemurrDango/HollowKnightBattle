using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ʒ�����Ч������
/// </summary>
public class DashEffectSpawner : Spawner
{
    [Header("-�����Ч")]
    [SerializeField]
    [Tooltip("������λ")]
    private Enemy belonger;
    [SerializeField]
    [Tooltip("�Ƿ�Ϊ���г����Ч")]
    private bool isAirDash;

    protected override GameObject SpawnInstance(GameObject prefab)
    {
        var go = base.SpawnInstance(prefab);
        if(isAirDash)go.transform.localRotation = belonger.transform.localRotation;
        go.transform.localScale = belonger.transform.localScale;
        return go;
    }
}
