using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioEvent),true)]
public class AudioEventPreviewEditor : Editor
{

	public AudioSource _previewer;

	public void OnEnable()
	{
		_previewer = EditorUtility.CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
		Debug.Log("Enab");
		Debug.Log("Prew " + _previewer);
	}

	public void OnDisable()
	{
		DestroyImmediate(_previewer.gameObject);
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
		if (GUILayout.Button("Preview"))
		{
			((AudioEvent) target).Play(_previewer);
		}
		EditorGUI.EndDisabledGroup();
	}
}
