
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(PlayerCustomState))]
public class StateDrawerUIE : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        int indent = EditorGUI.indentLevel;

        EditorGUI.indentLevel = 0;

        Rect oneRect = new Rect(position.x, position.y, position.width * 0.4f - 5, position.height);
        Rect twoRect = new Rect(position.x+ position.width*0.4f, position.y, position.width * 0.4f, position.height);

        SerializedProperty valueProp = property.FindPropertyRelative("stateValue");
        SerializedProperty nameProp = property.FindPropertyRelative("stateName");


        EditorGUI.PropertyField(oneRect, nameProp, GUIContent.none);
        EditorGUI.PropertyField(twoRect, valueProp, GUIContent.none);

        EditorGUI.indentLevel = 0;

        EditorGUI.EndProperty();
    }


}
