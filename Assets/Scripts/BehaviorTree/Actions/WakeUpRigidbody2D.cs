using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using UnityEngine;

[TaskName("休眠/唤醒")]
[TaskCategory("Unity/Rigidbody2D")]
[TaskDescription("休眠/唤醒目标2D刚体")]
public class WakeUpRigidbody2D : Action
{
    [TT("要操作的2D刚体所在的游戏对象，若不指定则在行为树所在对象上寻找")]
    public SharedGameObject targetGameObject;
    [TT("设置2D刚体的活跃性")]
    public bool active;

    private Rigidbody2D rigidbody2D;

    public override TaskStatus OnUpdate()
    {
        if (targetGameObject.Value == null) rigidbody2D = GetComponent<Rigidbody2D>();
        else rigidbody2D = targetGameObject.Value.GetComponent<Rigidbody2D>();
        if (active) rigidbody2D.WakeUp();else rigidbody2D.Sleep();
        return TaskStatus.Success;
    }
}
