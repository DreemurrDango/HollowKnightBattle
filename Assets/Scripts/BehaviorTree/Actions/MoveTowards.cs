using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using UnityEngine.Networking.Types;

[TaskName("向...移动")]
[TaskDescription("对对象施力使其对象向指定位置进行一次移动")]
/// <summary>
/// 使当前对象向
/// </summary>
public class MoveTowards: Action
{
    [TT("要移动到的目标形变位置")]
    public SharedTransform targetT;
    [TT("要移动到的目标位置，如果同时指定了目标形变，则为相对目标形变的偏移位置")]
	public SharedVector2 targetPos;
    [TT("力强度"), Min(0.1f)]
    public SharedFloat force;
	[TT("是否要旋转形变，使与面向前进方向对齐")]
	public bool doRotate;

	/// <summary>
	/// 该对象上的刚体组件
	/// </summary>
	private Rigidbody2D rigidbody2D;
    /// <summary>
    /// 本次任务需要移动的位置
    /// </summary>
	private Vector3 aimPos;
    public override void OnAwake()
    {
        rigidbody2D = Owner.GetComponent<Rigidbody2D>();
    }

    public override TaskStatus OnUpdate()
	{
        aimPos = targetT.Value != null ? targetT.Value.position + (Vector3)targetPos.Value : targetPos.Value;
        Vector3 direction = (aimPos - transform.position).normalized;
        if (doRotate)
        {
            // 计算角度
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle = (angle + 360) % 360;
            if (angle > 90 && angle < 270)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                angle -= 180;
            }
            else transform.localScale = new Vector3(1f, 1f, 1f);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        rigidbody2D.AddForce(direction * force.Value);
        return TaskStatus.Success;
	}
}