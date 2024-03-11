using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("����Ŀ��λ��")]
/// <summary>
/// ��Ծ��Ŀ��λ��
/// </summary>
public class JumpTo : Action
{
    /// <summary>
    /// ˮƽ������ϵ��
    /// </summary>
    public static float horizontalForceScale = 36f;

    [TT("Ҫ��Ծ����λ�õ�Ŀ���α�")]
    public SharedTransform aimT;
    [TT("��ֱ��Ծ����С��Χ��Ӱ�������Ծ�߶�")]
    public SharedVector2 verticalForceRange;

    /// <summary>
    /// �ö����ϵĸ������
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