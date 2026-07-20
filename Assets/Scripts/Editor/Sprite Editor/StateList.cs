using Haztech.SpriteEditor.Data;
using UnityEditor;
using UnityEngine;
namespace Haztech.SpriteEditor.Editor
{
    public static class StateList
    {
        [SerializeField] private static Vector2 scroll;
        public static void Draw(ToolWindow window, float height)
        {
            SpriteConfig config = window.SpriteConfig;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(height));
            GUILayout.Label("States", EditorStyles.boldLabel);


            Rect listRect = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(listRect, new Color(0.2f, 0.2f, 0.2f));

            scroll = EditorGUILayout.BeginScrollView(scroll);

            //if (Event.current.type == EventType.MouseDown &&
            //            listRect.Contains(Event.current.mousePosition))
            //{
            //    window.selectedState = -1;
            //    Event.current.Use();
            //    window.Repaint();
            //}

            if (config != null && config.StateCount > 0)
            {
                for (int i = 0; i < config.StateCount; i++)
                {
                    DrawStateRow(window, i);
                }
                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

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


            GUILayout.EndVertical();
        }

        private static void DrawStateRow(ToolWindow window, int index)
        {
            SpriteConfig config = window.SpriteConfig;

            Rect rowRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
            bool selected = config.selectedState == index;

            StateConfig state = config.GetStateConfig(index);
            if (state == null) return;

            // Draw state rect
            if (selected)
            {
                EditorGUI.DrawRect(
                    rowRect,
                    new Color(0.24f, 0.48f, 0.85f));
            }
            else if (rowRect.Contains(Event.current.mousePosition))
            {
                EditorGUI.DrawRect(
                    rowRect,
                    new Color(1f, 1f, 1f, 0.08f));
            }

            const float padding = 2f;
            const float buttonSize = 18f;

            Rect eyeRect = new Rect(
                rowRect.x + padding,
                rowRect.y,
                buttonSize,
                rowRect.height);

            Rect downRect = new Rect(
                rowRect.xMax - buttonSize - padding,
                rowRect.y,
                buttonSize,
                rowRect.height);

            Rect upRect = new Rect(
                downRect.x - buttonSize - padding,
                rowRect.y,
                buttonSize,
                rowRect.height);

            Rect labelRect = new Rect(
                eyeRect.xMax + 4f,
                rowRect.y,
                upRect.x - eyeRect.xMax - 8f,
                rowRect.height);

            GUI.Label(
                    new Rect(
                        labelRect.x + 4f,
                        labelRect.y,
                        labelRect.width - 8f,
                        labelRect.height),
                        state.name);

            if (index > 0 && GUI.Button(upRect, EditorGUIUtility.IconContent("scrollup"), EditorStyles.iconButton))
            {
                config.MoveLayerUp(index);
                config.selectedState--;
                EditorUtility.SetDirty(config);
                window.Repaint();
            }

            if (index < config.StateCount - 1 && GUI.Button(downRect, EditorGUIUtility.IconContent("scrolldown"), EditorStyles.iconButton))
            {
                config.MoveLayerDown(index);
                config.selectedState++;
                EditorUtility.SetDirty(config);
                window.Repaint();
            }

            if (Event.current.type == EventType.MouseDown &&
                rowRect.Contains(Event.current.mousePosition))
            {
                config.selectedState = index;
                Event.current.Use();
                window.Repaint();
            }
        }
    }
}