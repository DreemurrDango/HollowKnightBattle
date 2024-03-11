using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("跳跃")]
/// <summary>
/// 对于当前朝向后跳或向前跳
/// </summary>
public class Jump : Action
{
	[TT("水平跳跃力大小范围，影响跳跃过程在水平方向的位移")]
	public SharedVector2 horizontalForceRange;
    [TT("垂直跳跃力大小范围，影响最高跳跃高度")]
    public SharedVector2 verticalForceRange;
    [TT("基于当前水平面向，是向后还是向前跳")]
    public SharedBool backOrForward = true;

	/// <summary>
	/// 该对象上的刚体组件
	/// </summary>
	private Rigidbody2D rigidbody2D;

	private Vector2 h, v;
    public override void OnAwake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
		h = horizontalForceRange.Value; v = verticalForceRange.Value;
    }

	public override TaskStatus OnUpdate()
	{
		Vector2 force = new Vector2(Random.Range(h.x, h.y),Random.Range(v.x, v.y));
		force.x *= (backOrForward.Value ? 1 : -1) * rigidbody2D.transform.localScale.x;
		rigidbody2D.AddForce(force);
		return TaskStatus.Success;
	}
}