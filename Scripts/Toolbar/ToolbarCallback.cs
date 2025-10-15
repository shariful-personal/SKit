using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEngine.UIElements;

namespace SKit.Editor
{
	public static class ToolbarCallback
	{
		static Type m_toolbarType = typeof(EditorGUI).Assembly.GetType("UnityEditor.Toolbar");
		static ScriptableObject m_currentToolbar;

		public static Action OnToolbarGUI;
		public static Action OnToolbarGUILeft;
		public static Action OnToolbarGUIRight;
		
		static ToolbarCallback()
		{
			EditorApplication.update -= OnUpdate;
			EditorApplication.update += OnUpdate;
		}

		static void OnUpdate()
		{
			if (m_currentToolbar == null)
			{
				var toolbars = Resources.FindObjectsOfTypeAll(m_toolbarType);
				m_currentToolbar = toolbars.Length > 0 ? (ScriptableObject) toolbars[0] : null;
				if (m_currentToolbar != null)
				{
					var root = m_currentToolbar.GetType().GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
					var rawRoot = root.GetValue(m_currentToolbar);
					var mRoot = rawRoot as VisualElement;
					RegisterCallback("ToolbarZoneLeftAlign", OnToolbarGUILeft);
					RegisterCallback("ToolbarZoneRightAlign", OnToolbarGUIRight);

					void RegisterCallback(string root, Action cb) {
						var toolbarZone = mRoot.Q(root);

						var parent = new VisualElement()
						{
							style = {
								flexGrow = 1,
								flexDirection = FlexDirection.Row,
							}
						};
						var container = new IMGUIContainer();
						container.style.flexGrow = 1;
						container.onGUIHandler += () => { 
							cb?.Invoke();
						}; 
						parent.Add(container);
						toolbarZone.Add(parent);
					}
				}
			}
		}
	}
}