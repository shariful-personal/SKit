using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SKit.Editor
{
	[InitializeOnLoad]
	public static class ToolbarExtender
	{
		static int m_toolCount;
		static GUIStyle m_commandStyle = null;

		public static readonly List<Action> LeftToolbarGUI = new List<Action>();
		public static readonly List<Action> RightToolbarGUI = new List<Action>();

		static ToolbarExtender()
		{
			Type toolbarType = typeof(EditorGUI).Assembly.GetType("UnityEditor.Toolbar");
            string fieldName = "k_ToolCount";
			
			FieldInfo toolIcons = toolbarType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            m_toolCount = toolIcons != null ? ((int) toolIcons.GetValue(null)) : 8;
	
			ToolbarCallback.OnToolbarGUI = OnGUI;
			ToolbarCallback.OnToolbarGUILeft = GUILeft;
			ToolbarCallback.OnToolbarGUIRight = GUIRight;
		}

        public const float space = 8;
		public const float largeSpace = 20;
		public const float buttonWidth = 32;
		public const float dropdownWidth = 80;
		public const float playPauseStopWidth = 140;

		static void OnGUI()
		{
			if (m_commandStyle == null)
			{
				m_commandStyle = new GUIStyle("CommandLeft");
			}

			var screenWidth = EditorGUIUtility.currentViewWidth;

			float playButtonsPosition = Mathf.RoundToInt ((screenWidth - playPauseStopWidth) / 2);

			Rect leftRect = new(0, 0, screenWidth, Screen.height);
			leftRect.xMin += space; // Spacing left
			leftRect.xMin += buttonWidth * m_toolCount; // Tool buttons
			leftRect.xMin += space; // Spacing between tools and pivot
			leftRect.xMin += 64 * 2; // Pivot buttons
			leftRect.xMax = playButtonsPosition;

			Rect rightRect = new(0, 0, screenWidth, Screen.height);
			rightRect.xMin = playButtonsPosition;
			rightRect.xMin += m_commandStyle.fixedWidth * 3; // Play buttons
			rightRect.xMax = screenWidth;
			rightRect.xMax -= space; // Spacing right
			rightRect.xMax -= dropdownWidth; // Layout
			rightRect.xMax -= space; // Spacing between layout and layers
			rightRect.xMax -= dropdownWidth; // Layers
			rightRect.xMax -= space; // Spacing between layers and account
			rightRect.xMax -= dropdownWidth; // Account
			rightRect.xMax -= space; // Spacing between account and cloud
			rightRect.xMax -= buttonWidth; // Cloud
			rightRect.xMax -= space; // Spacing between cloud and collab
			rightRect.xMax -= 78; // Colab

			// Add spacing around existing controls
			leftRect.xMin += space;
			leftRect.xMax -= space;
			rightRect.xMin += space;
			rightRect.xMax -= space;

			// Add top and bottom margins
			leftRect.y = 4;
			leftRect.height = 22;
			rightRect.y = 4;
			rightRect.height = 22;

			if (leftRect.width > 0)
			{
				GUILayout.BeginArea(leftRect);
				GUILayout.BeginHorizontal();
				foreach (var handler in LeftToolbarGUI)
				{
					handler();
				}

				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}

			if (rightRect.width > 0)
			{
				GUILayout.BeginArea(rightRect);
				GUILayout.BeginHorizontal();
				foreach (var handler in RightToolbarGUI)
				{
					handler();
				}

				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}
		}
		
		public static void GUILeft() {
			GUILayout.BeginHorizontal();
			foreach (var handler in LeftToolbarGUI)
			{
				handler();
			}
			GUILayout.EndHorizontal();
		}
		
		public static void GUIRight() {
			GUILayout.BeginHorizontal();
			foreach (var handler in RightToolbarGUI)
			{
				handler();
			}
			GUILayout.EndHorizontal();
		}
	}
}
