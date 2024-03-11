using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField]
    [Tooltip("滑动条组件")]
    private Slider slider;
    [SerializeField]
    [Tooltip("滑动条滑块的贴图")]
    private Image handleImage;
    [SerializeField]
    [Tooltip("滑动条滑动速度")]
    private float speed = 0.5f;

    [SerializeField]
    [Tooltip("指向的敌人数据")]
    private Enemy enemy; 

    /// <summary>
    /// 所指向敌人的当前生命值
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
    /// 对照数据进行刷新
    /// </summary>
    public void LoadData(bool showTranstion)
    {
        nowHealth = enemy.Health;
        float newValue = (enemy.FullHealth - nowHealth) / enemy.FullHealth;
        if (!showTranstion) slider.value = newValue;
        else slider.DOValue(newValue, (newValue - slider.value) / speed);
    }

    /// <summary>
    /// 切换当前指向的敌人目标
    /// </summary>
    /// <param name="e"></param>
    public void SwitchTarget(Enemy e)
    {
        enemy?.afterLoseHealth.RemoveListener(() => LoadData(true));
        enemy = e;
        enemy?.afterLoseHealth.AddListener(() => LoadData(true));
    }

    /// <summary>
    /// 滑动条的数值，可通过该值直接设置滑动条数值
    /// </summary>
    public float Value
    {
        get => slider.value;
        set => slider.value = value;
    }
}
