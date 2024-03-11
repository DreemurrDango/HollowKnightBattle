using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField]
    [Tooltip("���������")]
    private Slider slider;
    [SerializeField]
    [Tooltip("�������������ͼ")]
    private Image handleImage;
    [SerializeField]
    [Tooltip("�����������ٶ�")]
    private float speed = 0.5f;

    [SerializeField]
    [Tooltip("ָ��ĵ�������")]
    private Enemy enemy; 

    /// <summary>
    /// ��ָ����˵ĵ�ǰ����ֵ
    /// </summary>
    private float nowHealth = 0;

    private void OnEnable()
    {
        enemy?.afterLoseHealth.AddListener(() => LoadData(true));
        slider.onValueChanged.AddListener(OnValueChange) ;
    }

    private void OnDisable()
    {
        enemy?.afterLoseHealth.RemoveListener(() => LoadData(true));
        slider.onValueChanged.RemoveListener(OnValueChange);
    }

    private void OnValueChange(float value)
    {
        if(value == 0f || value == 1)handleImage.enabled = false;
        else handleImage.enabled = true;
    }

    /// <summary>
    /// �������ݽ���ˢ��
    /// </summary>
    public void LoadData(bool showTranstion)
    {
        nowHealth = enemy.Health;
        float newValue = (enemy.FullHealth - nowHealth) / enemy.FullHealth;
        if (!showTranstion) slider.value = newValue;
        else slider.DOValue(newValue, (newValue - slider.value) / speed);
    }

    /// <summary>
    /// �л���ǰָ��ĵ���Ŀ��
    /// </summary>
    /// <param name="e"></param>
    public void SwitchTarget(Enemy e)
    {
        enemy?.afterLoseHealth.RemoveListener(() => LoadData(true));
        enemy = e;
        enemy?.afterLoseHealth.AddListener(() => LoadData(true));
    }

    /// <summary>
    /// ����������ֵ����ͨ����ֱֵ�����û�������ֵ
    /// </summary>
    public float Value
    {
        get => slider.value;
        set => slider.value = value;
    }
}
