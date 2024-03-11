using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(EnumName))]
public class EnumNameDrawer : PropertyDrawer
{
    private readonly List<string> displayNames = new List<string>();
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var ena = attribute as EnumName;
        var type = property.serializedObject.targetObject.GetType();
        var field = type.GetField(property.name);
        var enumType = field.FieldType;
        foreach(var enumName in property.enumNames)
        {
            var enumField = enumType.GetField(enumName);
            var hds = enumField.GetCustomAttributes(typeof(HeaderAttribute), false);
            displayNames.Add(hds.Length <= 0 ? enumName : ((HeaderAttribute)hds[0]).header);
        }
        EditorGUI.BeginChangeCheck();
        var value = EditorGUI.Popup(position, ena.header, property.enumValueIndex, displayNames.ToArray());
        if (EditorGUI.EndChangeCheck()) property.enumValueIndex = value;
    }
}
