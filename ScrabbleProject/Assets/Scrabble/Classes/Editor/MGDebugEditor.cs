using UnityEngine;
using UnityEditor;
using System.Collections;

namespace MGEditor
{
	using MGTools;

	[CustomEditor(typeof(MGDebug))]
	public class MGDebugEditor : Editor 
	{
		public override void OnInspectorGUI ()
		{
			DrawDefaultInspector();
			MGDebug editor = target as MGDebug;
			
			EditorGUILayout.BeginHorizontal();
			{
				if (DrawButton("Validate", "Validate the characters entered!", 40f))
				{
					editor.ValidateLetters();
				}
			}
			EditorGUILayout.EndHorizontal();
		}
		
		static bool DrawButton (string title, string tooltip, float width)
		{
			return GUILayout.Button(new GUIContent(title, tooltip));
		}
	}
}