using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Cinemachine;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("目标切换")]
[TaskCategory("摄像机")]
[TaskDescription("切换摄像机的注视目标")]
public class CameraTargetChange : Action
{

    [TT("目标组组件所在的游戏对象，若不指定则为行为树所在对象")]
    public SharedGameObject targetGroupGO;
    [TT("对原目标组的操作方式 F:移除/T：新增")]
    public bool removeOrAdd;

    [TT("要添加的形变，若不指定则为行为树所在对象的形变")]
    public SharedTransform valueT;
    [TT("要添加的新数据的权重")]
    public float weight = 1f;

    /// <summary>
    /// 摄像机追踪目标组组件
    /// </summary>
    private CinemachineTargetGroup targetGroup;
    public override void OnAwake()
    {
        targetGroup = targetGroupGO.Value?.GetComponent<CinemachineTargetGroup>();
        if(valueT.Value == null) valueT.Value = Owner.transform;
    }
    public override TaskStatus OnUpdate()
    {
        if (targetGroup == null || valueT.Value == null) return TaskStatus.Failure;
        if (!removeOrAdd) targetGroup.RemoveMember(valueT.Value);
        else targetGroup.AddMember(valueT.Value, weight, 0f);
        return TaskStatus.Success;
    }
}
