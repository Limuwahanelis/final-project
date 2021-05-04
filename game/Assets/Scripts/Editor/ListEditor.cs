using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public static class ListEditor
{
    public static void Show(SerializedProperty list)
    {
        //EditorGUILayout.PropertyField(list);
        EditorGUI.indentLevel += 1;
        for(int i=0;i<list.arraySize;i++)
        {
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
        }
        EditorGUI.indentLevel -= 1;
    }
}
