using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("角色控制锁")]
[TaskCategory("主角")]
[TaskDescription("设置角色的不可控制状态时间，使其可/不可控制")]
public class SetMainControlLock : Action
{
    [TT("挂载有角色控制器组件的游戏对象，若置空则自动寻找单例对象")]
    public SharedGameObject mainCharaGO;
    [TT("要设置该敌人是否可被攻击的值，设为0可解决控制锁")]
    public float controlLockTime;

    /// <summary>
    /// 敌人脚本
    /// </summary>
    private CharaController mainChara;

    public override void OnAwake()
    {
        if (mainCharaGO.Value == null) mainChara = CharaController.Instance;
        else mainChara = mainCharaGO.Value.GetComponent<CharaController>();
    }

    public override TaskStatus OnUpdate()
    {
        mainChara.SetControlLock(controlLockTime);
        return TaskStatus.Success;
    }
}