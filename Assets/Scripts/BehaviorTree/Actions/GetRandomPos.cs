using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.ObjectDrawers;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using UnityEditor;
using Enums;

[TaskName("获取随机点")]
[TaskDescription("获取以目标形变为中心的形状范围内的任意一点(当前只支持弧形、盒型范围)")]
public class GetRandomPos : Action
{
    [TT("选点区域的形状")]
    public RangeShape rangeShape;
	[TT("目标形变，作为中心位置；若不指定则为当前行为树所在对象的形变")]
	public SharedTransform centreT;
    [TT("对于目标形变的偏移值")]
    public Vector2 offset;
    [TT("仅当形状为扇形或圆形时有效：圆的半径大小"),Min(0.1f)]
    public float radius;
	[TT("仅当形状为扇形时有效：弧形角度值范围，应当为0~360")]
	public Vector2 angleRange;
    [TT("仅当形状为扇形或圆形时有效：圆的半径大小"), Min(0.1f)]
    public Vector2 size;
    [TT("获取到的随机点")]
    public SharedVector2 returnPos;

	/// <summary>
	/// 最终使用的中心位置形变
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
        Debug.Log("冲刺位置为" + returnPos.Value);
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