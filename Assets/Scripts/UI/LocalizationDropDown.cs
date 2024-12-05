using System;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
/// <summary>
/// ��Ӧ����ı��ػ�����ѡ����������ű�
/// Ӧ�������������˵���ͬһ������
/// </summary>
public class LocalizationDropDown : MonoBehaviour
{
    /// <summary>
    /// �ö����ϵ����������
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
    /// �����˵�ѡ�����µ�ѡ��ʱ�¼�
    /// </summary>
    /// <param name="index"></param>
    private void OnDropDownValueChanged(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}
