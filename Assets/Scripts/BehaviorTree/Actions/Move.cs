using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("ˮƽ�ƶ�")]
/// <summary>
/// ˮƽ�ƶ���ǰ����
/// </summary>
public class Move : Action
{
	[TT("Ҫˮƽ�ƶ��ľ��뷶Χ")]
	public SharedVector2 horizontalVectorRange;
    [TT("�ƶ��ٶ�"), Min(0.1f)]
    public SharedFloat speed;
	[TT("�Ƿ�Ҫ��������ʹ��ǰ���������")]
	public bool changeFace;

	/// <summary>
	/// �ö����ϵĸ������
	/// </summary>
	private Rigidbody2D rigidbody2D;
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