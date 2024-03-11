using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("跳到目标位置")]
/// <summary>
/// 跳跃到目标位置
/// </summary>
public class JumpTo : Action
{
    /// <summary>
    /// 水平力缩放系数
    /// </summary>
    public static float horizontalForceScale = 36f;

    [TT("要跳跃到其位置的目标形变")]
    public SharedTransform aimT;
    [TT("垂直跳跃力大小范围，影响最高跳跃高度")]
    public SharedVector2 verticalForceRange;

    /// <summary>
    /// 该对象上的刚体组件
    /// </summary>
    private Rigidbody2D rigidbody2D;
    public override void OnStart()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

	public override TaskStatus OnUpdate()
	{
        Vector2 force;
        force.x = (aimT.Value.position - rigidbody2D.transform.position).x / rigidbody2D.mass * horizontalForceScale;
        force.y = Random.Range(verticalForceRange.Value.x,verticalForceRange.Value.y);
        rigidbody2D.AddForce(force);
        return TaskStatus.Success;
    }
}