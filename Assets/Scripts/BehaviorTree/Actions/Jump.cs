using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("��Ծ")]
/// <summary>
/// ���ڵ�ǰ�����������ǰ��
/// </summary>
public class Jump : Action
{
	[TT("ˮƽ��Ծ����С��Χ��Ӱ����Ծ������ˮƽ�����λ��")]
	public SharedVector2 horizontalForceRange;
    [TT("��ֱ��Ծ����С��Χ��Ӱ�������Ծ�߶�")]
    public SharedVector2 verticalForceRange;
    [TT("���ڵ�ǰˮƽ�������������ǰ��")]
    public SharedBool backOrForward = true;

	/// <summary>
	/// �ö����ϵĸ������
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