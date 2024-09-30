using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("�ƶ���")]
/// <summary>
/// ����ǰ�����ƶ���ָ���α�λ��
/// </summary>
public class MoveTo : Action
{
    [TT("Ҫ�ƶ�����Ŀ���α�λ��")]
    public SharedTransform targetT;
    [TT("��ָ����Ŀ���α䣬�Ƿ����ƶ������и���֮����Ϊ����ֻ���ƶ�������ʼʱĿ���α����ڵ�λ��")]
    public bool doFollow;
    [TT("Ҫ�ƶ�����Ŀ��λ�ã����ͬʱָ����Ŀ���α䣬��Ϊ���Ŀ���α��ƫ��λ��")]
	public SharedVector2 targetPos;
    [TT("��Ŀ��λ�þ���С�ڶ���ʱ��Ϊ����")]
    public float minDistance = 0.2f;
    [TT("�ƶ��ٶ�"), Min(0.1f)]
    public SharedFloat speed;
	[TT("�Ƿ�Ҫ��ת�α䣬ʹ������ǰ���������")]
	public bool doRotate;

	/// <summary>
	/// �ö����ϵĸ������
	/// </summary>
	private Rigidbody2D rigidbody2D;
    /// <summary>
    /// ����������ƶ�Ŀ���α�
    /// </summary>
    private Transform aimT;
    /// <summary>
    /// ����������Ҫ�ƶ���λ��
    /// </summary>
	private Vector2 aimPos;
    /// <summary>
    /// ÿ���ƶ�������
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