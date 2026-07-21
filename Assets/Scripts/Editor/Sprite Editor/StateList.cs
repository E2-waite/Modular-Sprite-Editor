using Haztech.SpriteEditor.Data;
using UnityEditor;
using UnityEngine;

namespace Haztech.SpriteEditor.Editor
{
    public static class StateList
    {
        [SerializeField] private static Vector2 scroll;
        public static void Draw(float height)
        {
            SpriteConfig config = ToolWindow.Instance.SpriteConfig;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(height));
            GUILayout.Label("States", EditorStyles.boldLabel);

            DrawList(config);

            EditorGUILayout.Space();

            DrawButtons(config);

            GUILayout.EndVertical();
        }

        private static void DrawList(SpriteConfig config)
        {
            Rect listRect = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(listRect, new Color(0.2f, 0.2f, 0.2f));

            scroll = EditorGUILayout.BeginScrollView(scroll);

            if (config != null && config.StateCount > 0)
            {
                for (int i = 0; i < config.StateCount; i++)
                {
                    StateRow.Draw(i);
                }
                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private static void DrawButtons(SpriteConfig config)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create") && config != null)
            {
                Undo.RecordObject(config, "Create Sprite State");
                config.AddState(new StateConfig("New State"));
                config.selectedState = config.StateCount - 1;
                EditorUtility.SetDirty(config);
            }

            using (new EditorGUI.DisabledScope(config.StateCount <= 1))
            {
                if (GUILayout.Button("Remove") && config != null)
                {
                    Undo.RecordObject(config, "Remove Sprite State");
                    config.RemoveState(config.selectedState);
                    config.selectedState = config.StateCount - 1;
                    EditorUtility.SetDirty(config);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}