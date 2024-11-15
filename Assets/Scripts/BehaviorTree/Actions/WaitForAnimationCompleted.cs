using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskCategory("Unity/Animator")]
[TaskName("等待动画播放完成")]
[TaskDescription("等待指定动画状态的动画完成一次播放，一般用于等待非循环动画状态播放结束")]
public class WaitForAnimationCompleted : Action
{
    [TT("要检测的动画机所位于的游戏对象，若不指定则默认为行为树所在对象上的动画器组件")]
    public SharedGameObject targetGameObject;
    [TT("要检测的动画状态名，若置为空将等待当前动画状态运行结束")]
    public string stateName;
    [TT("当状态名与指定名不同时是否继续等待，若设为false则在状态不同时直接返回失败")]
    public bool waitOnStateDiffer;

    private Animator animator;
    private int stateHash;

    public override void OnAwake()
    {
        animator = (targetGameObject.Value != null ? targetGameObject.Value : Owner.gameObject).GetComponent<Animator>();
        if (stateName != "")
        {
            stateHash = Animator.StringToHash(stateName);
            if(!animator.HasState(0, Animator.StringToHash(stateName)))
                Debug.LogError("动画器上不存在名为“ " + stateName + "”的动画状态");
        }
    }

    public override TaskStatus OnUpdate()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateName == "") return stateInfo.normalizedTime >= 1 ? TaskStatus.Success : TaskStatus.Running;
        else if (stateInfo.shortNameHash != stateHash) return waitOnStateDiffer ? TaskStatus.Running : TaskStatus.Failure;
        else return stateInfo.normalizedTime >= 0.85f ? TaskStatus.Success : TaskStatus.Running;
    }

    public override void OnReset()
    {
        targetGameObject = null;
        stateName = "";
    }
}
