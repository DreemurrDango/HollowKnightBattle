using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("移动到")]
/// <summary>
/// 将当前对象移动到指定形变位置
/// </summary>
public class MoveTo : Action
{
    [TT("要移动到的目标形变位置")]
    public SharedTransform targetT;
    [TT("若指定了目标形变，是否在移动过程中跟随之，若为否，则只是移动到任务开始时目标形变所在的位置")]
    public bool doFollow;
    [TT("要移动到的目标位置，如果同时指定了目标形变，则为相对目标形变的偏移位置")]
	public SharedVector2 targetPos;
    [TT("与目标位置距离小于多少时视为到达")]
    public float minDistance = 0.2f;
    [TT("移动速度"), Min(0.1f)]
    public SharedFloat speed;
	[TT("是否要旋转形变，使与面向前进方向对齐")]
	public bool doRotate;

	/// <summary>
	/// 该对象上的刚体组件
	/// </summary>
	private Rigidbody2D rigidbody2D;
    /// <summary>
    /// 本次任务的移动目标形变
    /// </summary>
    private Transform aimT;
    /// <summary>
    /// 本次任务需要移动的位置
    /// </summary>
	private Vector2 aimPos;
    /// <summary>
    /// 每次移动的向量
    /// </summary>
    private Vector2 moveVector;
    public override void OnAwake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public override void OnStart()
    {
        aimT = targetT.Value;
        aimPos = aimT != null ? (Vector2)aimT.position + targetPos.Value : targetPos.Value;
        transform.LookAt(aimPos);
        moveVector = (aimPos - (Vector2)transform.position).normalized * speed.Value;
    }

    public override TaskStatus OnUpdate()
	{
        if(doFollow)
        {
            aimPos = aimT != null ? (Vector2)aimT.position + targetPos.Value : targetPos.Value;
            transform.LookAt(aimPos);
            moveVector = (aimPos - (Vector2)transform.position).normalized * speed.Value;
        }
        rigidbody2D.velocity = moveVector;
        if (Vector2.Distance(rigidbody2D.position, aimPos) < minDistance) return TaskStatus.Success;
        else return TaskStatus.Running;
	}
}