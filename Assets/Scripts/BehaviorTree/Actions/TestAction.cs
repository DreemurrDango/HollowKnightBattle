using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.ObjectDrawers;

[TaskCategory("����")]
[TaskIcon("Assets/Resources/Texture/Effects/fk-shockwave.png")]
[TaskName("������ʾ��")]
[TaskDescription("������ʾ�����������Ϣ")]
[BehaviorDesigner.Runtime.Tasks.HelpURL("www.baidu.com")]
[BehaviorDesigner.Runtime.Tasks.RequiredComponent(typeof(Rigidbody2D))]
public class TestAction : Action
{
	[FloatSlider(0f,10f)]
	public float value;
	[IntSlider(-10,10)]
	public int value2;
    [BehaviorDesigner.Runtime.Tasks.InspectTask]
    public Vector2 vector;
	[BehaviorDesigner.Runtime.Tasks.RequiredField]
	public GameObject @object;
	[BehaviorDesigner.Runtime.Tasks.SharedRequired]
	public Transform t;
    public override void OnStart()
	{
		
	}

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}
}