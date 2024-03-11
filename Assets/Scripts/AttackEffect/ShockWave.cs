using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地震波攻击特效的移动功能
/// </summary>
public class ShockWave : MonoBehaviour
{
    [SerializeField]
    [Tooltip("地震波速度随时间变化的曲线")]
    private AnimationCurve speedCurve;

    [Header("//观察窗口")]
    [SerializeField,DisplayOnly]
    [Tooltip("当前速度值")]
    private float currentSpeed;
    [SerializeField, DisplayOnly]
    [Tooltip("方向单位量")]
    private Vector3 direction;

    /// <summary>
    /// 该对象已存在的时间
    /// </summary>
    private float t_live;
    /// <summary>
    /// 初始化数据:设置方向变量
    /// </summary>
    public void Init(Vector3 direction)
    {
        this.direction = direction;
    }

    private void Start()
    {
        t_live = 0f;
    }

    private void Update()
    {
        currentSpeed = speedCurve.Evaluate(t_live);
        transform.position += direction * currentSpeed * Time.deltaTime;
        t_live += Time.deltaTime;
    }
}
