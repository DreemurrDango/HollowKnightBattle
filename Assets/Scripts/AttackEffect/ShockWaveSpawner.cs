using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveSpawner : Spawner
{
    [Header("-³å»÷²¨")]
    [SerializeField]
    [Tooltip("ËùÊôµ¥Î»")]
    private Enemy belonger;

    protected override GameObject SpawnInstance(GameObject prefab)
    {
        var go = base.SpawnInstance(prefab);
        var sw = go.GetComponent<ShockWave>();
        sw.Init(Vector3.right * (belonger.DoFaceRight ? 1 : -1));
        return go;
    }
}
