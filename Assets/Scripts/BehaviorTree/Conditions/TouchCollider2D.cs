using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Enums;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using System.Collections.Generic;

/// <summary>
/// �жϵ��˵�����ֵ�Ƿ���������
/// </summary>
[TaskName("�Ƿ���ײ")]
[TaskDescription("�жϵ�ǰ�Ƿ���ָ����2D��ײ�л��ǩ������ײ")]
public class TouchCollider2D : Conditional
{
	[TT("Ҫ����2D��ײ��������ָ��������Ϊ�����ڶ�����Ѱ�����")]
	public Collider2D collider2D;
    [TT("Ҫ����Ƿ�����ײ����һ2D��ײ�������ǩ����ͬʱΪ��")]
    public Collider2D other;
    [TT("Ҫ������ײĿ������ͼ��")]
    public LayerMask layerMask;
    [TT("Ҫ����Ƿ�����ײ�ı�ǩ")]
    public string tag;
    [TT("�Ƿ�Խ��ȡ��")]
    public bool invertResult = false;

    public override void OnAwake()
    {
        if(collider2D == null ) collider2D = Owner.GetComponent<Collider2D>();
        if(layerMask == Physics2D.AllLayers &&tag == "" && other == null) Debug.LogError("δ������Ч�Ĳ���");
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