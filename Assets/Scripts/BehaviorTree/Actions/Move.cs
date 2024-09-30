using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("水平移动")]
/// <summary>
/// 水平移动当前对象
/// </summary>
public class Move : Action
{
	[TT("要水平移动的距离范围")]
	public SharedVector2 horizontalVectorRange;
    [TT("移动速度"), Min(0.1f)]
    public SharedFloat speed;
	[TT("是否要更改面向，使与前进方向对齐")]
	public bool changeFace;

	/// <summary>
	/// 该对象上的刚体组件
	/// </summary>
	private Rigidbody2D rigidbody2D;
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
        Vector2 h = horizontalVectorRange.Value;
        float x = Random.Range(h.x, h.y);
        aimPos = rigidbody2D.position + new Vector2(x, 0);
        moveVector = new Vector2((x > 0 ? 1 : -1) * speed.Value, 0);
        if (changeFace) rigidbody2D.transform.localScale = new Vector3(x > 0 ? 1 : -1, 1, 1);
        Debug.Log(aimPos);
    }

    public override TaskStatus OnUpdate()
	{
        Vector2 v = rigidbody2D.velocity;
        v.x = moveVector.x;
        rigidbody2D.velocity = v;
        if (Mathf.Abs(rigidbody2D.position.x - aimPos.x) < 0.2f) return TaskStatus.Success;
        else return TaskStatus.Running;
	}
}