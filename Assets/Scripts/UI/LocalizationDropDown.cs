using System;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
/// <summary>
/// 响应与更改本地化语言选项的下拉栏脚本
/// 应当挂载在下拉菜单的同一对象上
/// </summary>
public class LocalizationDropDown : MonoBehaviour
{
    /// <summary>
    /// 该对象上的下拉栏组件
    /// </summary>
    private TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(OnDropDownValueChanged);
    }

    private void OnEnable()
    {
        dropdown.value = LocalizationSettings.SelectedLocale.SortOrder;
    }

    /// <summary>
    /// 下拉菜单选择了新的选项时事件
    /// </summary>
    /// <param name="index"></param>
    private void OnDropDownValueChanged(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}
