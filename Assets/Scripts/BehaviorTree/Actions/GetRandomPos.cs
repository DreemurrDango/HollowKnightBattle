using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.ObjectDrawers;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using UnityEditor;
using Enums;

[TaskName("��ȡ�����")]
[TaskDescription("��ȡ��Ŀ���α�Ϊ���ĵ���״��Χ�ڵ�����һ��(��ǰֻ֧�ֻ��Ρ����ͷ�Χ)")]
public class GetRandomPos : Action
{
    [TT("ѡ���������״")]
    public RangeShape rangeShape;
	[TT("Ŀ���α䣬��Ϊ����λ�ã�����ָ����Ϊ��ǰ��Ϊ�����ڶ�����α�")]
	public SharedTransform centreT;
    [TT("����Ŀ���α��ƫ��ֵ")]
    public Vector2 offset;
    [TT("������״Ϊ���λ�Բ��ʱ��Ч��Բ�İ뾶��С"),Min(0.1f)]
    public float radius;
	[TT("������״Ϊ����ʱ��Ч�����νǶ�ֵ��Χ��Ӧ��Ϊ0~360")]
	public Vector2 angleRange;
    [TT("������״Ϊ���λ�Բ��ʱ��Ч��Բ�İ뾶��С"), Min(0.1f)]
    public Vector2 size;
    [TT("��ȡ���������")]
    public SharedVector2 returnPos;

	/// <summary>
	/// ����ʹ�õ�����λ���α�
	/// </summary>
    private Transform ct;
    public override void OnStart()
	{
		ct = centreT.Value != null ? centreT.Value : Owner.transform;
	}

	public override TaskStatus OnUpdate()
	{
        float r, a, x, y;
        switch (rangeShape)
        {
            case RangeShape.point:break;
            case RangeShape.box2D:
                x = Random.Range(-1 * size.x, size.x);
                y = Random.Range(-1 * size.y, size.y);
                returnPos.SetValue((Vector2)ct.position + offset + new Vector2(x, y));
                break;
            case RangeShape.box:break;
            case RangeShape.circle:
                r = Random.Range(0, radius);
                returnPos.SetValue((Vector2)ct.position + offset + r * Random.insideUnitCircle);
                break;
            case RangeShape.fan:
                r = Random.Range(0, radius); a = Random.Range(angleRange.x, angleRange.y);
                returnPos.SetValue((Vector2)ct.position + offset + r * new Vector2(Mathf.Cos(a * Mathf.Deg2Rad), Mathf.Sin(a * Mathf.Deg2Rad)));
                break;
            case RangeShape.sphere:break;
        }
        Debug.Log("���λ��Ϊ" + returnPos.Value);
        Debug.DrawLine(Owner.transform.position, returnPos.Value);
		return TaskStatus.Success;
	}

    public override void OnDrawGizmos()
    {
#if UNITY_EDITOR
        var oldColor = Handles.color;
        var color = Color.yellow;
        color.a = 0.1f;
        Handles.color = color;

        Vector3 c = (centreT.Value != null ? centreT.Value : Owner.transform).position + (Vector3)offset;
        switch (rangeShape)
        {
            case RangeShape.box2D:
                Handles.DrawSolidRectangleWithOutline(new Rect(c - (Vector3)size/2, size), color, Color.black);
                break;
            case RangeShape.circle:
                Handles.DrawSolidArc(c, Vector3.forward, Vector3.right, 360, radius);
                break;
            case RangeShape.fan:
                var beginDirection = new Vector3(Mathf.Cos(angleRange.x * Mathf.Deg2Rad), Mathf.Sin(angleRange.x * Mathf.Deg2Rad), 0f);
                Handles.DrawSolidArc(c, Vector3.forward, beginDirection, angleRange.y - angleRange.x, radius);
                break;
        }
        Handles.color = oldColor;
#endif
    }
}