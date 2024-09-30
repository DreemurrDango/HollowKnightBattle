using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Enums;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using System.Collections.Generic;

/// <summary>
/// 判断敌人的生命值是否满足条件
/// </summary>
[TaskName("是否碰撞")]
[TaskDescription("判断当前是否与指定的2D碰撞盒或标签物体碰撞")]
public class TouchCollider2D : Conditional
{
	[TT("要检测的2D碰撞器，若不指定则在行为树所在对象上寻找组件")]
	public Collider2D collider2D;
    [TT("要检测是否发生碰撞的另一2D碰撞器，与标签不可同时为空")]
    public Collider2D other;
    [TT("要检测的碰撞目标所处图层")]
    public LayerMask layerMask;
    [TT("要检测是否发生碰撞的标签")]
    public string tag;
    [TT("是否对结果取反")]
    public bool invertResult = false;

    public override void OnAwake()
    {
        if(collider2D == null ) collider2D = Owner.GetComponent<Collider2D>();
        if(layerMask == Physics2D.AllLayers &&tag == "" && other == null) Debug.LogError("未设置有效的参数");
    }

    public override TaskStatus OnUpdate()
    {
        //Debug.Log(collider2D.IsTouching(other));
        //if(collider2D != null) return collider2D.IsTouching(other) ? TaskStatus.Success : TaskStatus.Failure;
        ContactFilter2D filter2D = new ContactFilter2D();
        filter2D.SetLayerMask(layerMask);
        List<Collider2D> results = new List<Collider2D>();
        collider2D.OverlapCollider(filter2D, results);
        if(results.Count == 0 ) return TaskStatus.Failure;
        foreach(var c in results)
        {
            if (tag != "" && c.gameObject.tag == tag)
            {
                Debug.Log(c.gameObject);
                return TaskStatus.Success;
            }
        }
        return layerMask != Physics2D.AllLayers ? TaskStatus.Success : TaskStatus.Failure;
    }
}