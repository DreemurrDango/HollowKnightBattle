using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���𲨹�����Ч���ƶ�����
/// </summary>
public class ShockWave : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�����ٶ���ʱ��仯������")]
    private AnimationCurve speedCurve;

    [Header("//�۲촰��")]
    [SerializeField,DisplayOnly]
    [Tooltip("��ǰ�ٶ�ֵ")]
    private float currentSpeed;
    [SerializeField, DisplayOnly]
    [Tooltip("����λ��")]
    private Vector3 direction;

    /// <summary>
    /// �ö����Ѵ��ڵ�ʱ��
    /// </summary>
    private float t_live;
    /// <summary>
    /// ��ʼ������:���÷������
    /// </summary>
    public void Init(Vector3 direction)
    {
        this.direction = direction;
    }

    private void Start()
    {
        t_live = 0f;
    }

    private void Update()
    {
        currentSpeed = speedCurve.Evaluate(t_live);
        transform.position += direction * currentSpeed * Time.deltaTime;
        t_live += Time.deltaTime;
    }
}
