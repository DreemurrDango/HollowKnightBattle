using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using UnityEngine.Networking.Types;

[TaskName("��...�ƶ�")]
[TaskDescription("�Զ���ʩ��ʹ�������ָ��λ�ý���һ���ƶ�")]
/// <summary>
/// ʹ��ǰ������
/// </summary>
public class MoveTowards: Action
{
    [TT("Ҫ�ƶ�����Ŀ���α�λ��")]
    public SharedTransform targetT;
    [TT("Ҫ�ƶ�����Ŀ��λ�ã����ͬʱָ����Ŀ���α䣬��Ϊ���Ŀ���α��ƫ��λ��")]
	public SharedVector2 targetPos;
    [TT("��ǿ��"), Min(0.1f)]
    public SharedFloat force;
	[TT("�Ƿ�Ҫ��ת�α䣬ʹ������ǰ���������")]
	public bool doRotate;

	/// <summary>
	/// �ö����ϵĸ������
	/// </summary>
	private Rigidbody2D rigidbody2D;
    /// <summary>
    /// ����������Ҫ�ƶ���λ��
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
            // ����Ƕ�
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