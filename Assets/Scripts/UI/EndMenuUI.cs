using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Drawing;

/// <summary>
/// 结局菜单界面UI
/// </summary>
public class EndMenuUI : MonoBehaviour
{
    [Tooltip("所有要实现shader效果的TextMesh组件")]
    public TMP_Text[] texts;

    [Tooltip("字体的膨胀效果数值")]
    public float dilateValue;

    [Tooltip("已用时间显示文本")]
    public TMP_Text timeUsedInfoText;
    [SerializeField]
    [Tooltip("显示动画结束时的事件")]
    private UnityEvent onShowCompleted;

    private void Update()
    {
        foreach (TMP_Text t in texts)
            t.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, dilateValue);
    }

    public void OnShowAnimCompleted() => onShowCompleted?.Invoke();
    public void ShowTimeUsedInfo()
    {
        if (timeUsedInfoText == null) return;
        string result = string.Format("<color=yellow><b>{0}</b></color>", Time.timeSinceLevelLoad);
        timeUsedInfoText.text = result;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        foreach (TMP_Text t in texts)
            t.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, dilateValue);
    }
#endif
}
