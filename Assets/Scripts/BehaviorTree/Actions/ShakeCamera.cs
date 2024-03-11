using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Cinemachine;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("摄像机震动")]
/// <summary>
/// 设置震动源的方向与力度来震动摄像机
/// </summary>
public class ShakeCamera : Action
{
    [TT("震动源组件所在的游戏对象")]
    public SharedGameObject impulseSourceGO;
    [TT("震动方向")]
	public Vector2 direction;
    [TT("震动力度")]
    public float force;
    /// <summary>
    /// 最终将使用的震动源组件
    /// </summary>
    private CinemachineImpulseSource impulseSource;

    public override void OnAwake()
    {
        impulseSource = impulseSourceGO.Value?.GetComponent<CinemachineImpulseSource>();
        if (impulseSource == null) Debug.LogWarning("未指定震动源组件！");
    }

    public override TaskStatus OnUpdate()
	{
        impulseSource.GenerateImpulse(force * direction);
		return TaskStatus.Success;
	}
}