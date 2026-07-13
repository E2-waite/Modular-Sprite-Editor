using SpriteEditor.Data;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace SpriteEditor.Editor
{
    public static class SpriteEditorLayerPanel
    {
        [SerializeField] private static Vector2 layerScroll;
        [SerializeField] private static Vector2 stateScroll;

        public static void Draw(SpriteEditorWindow window)
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(220),
                 GUILayout.ExpandHeight(true));

            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float sectionHeight = (window.position.height - spacing) * 0.5f;


            DrawLayers(window, sectionHeight);

            GUILayout.Space(spacing);

            DrawStates(window, sectionHeight);

            EditorGUILayout.EndVertical();
        }

        private static void DrawLayers(SpriteEditorWindow window, float height)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(height));
            GUILayout.Label("Layers", EditorStyles.boldLabel);


            Rect listRect = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(listRect, new Color(0.2f, 0.2f, 0.2f));

            layerScroll = EditorGUILayout.BeginScrollView(layerScroll);

            //if (Event.current.type == EventType.MouseDown &&
            //            listRect.Contains(Event.current.mousePosition))
            //{
            //    window.selectedLayer = -1;
            //    Event.current.Use();
            //    window.Repaint();
            //}

            if (window.Config != null && window.Config.LayerCount > 0)
            {
                for (int i = 0; i < window.Config.LayerCount; i++)
                {
                    DrawLayerRow(window, i);
                }
                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Create") && window.Config != null)
            {
                Undo.RecordObject(window.Config, "Create Sprite Layer");
                window.Config.AddLayer(new SpriteLayer());
                window.selectedLayer = window.Config.LayerCount - 1;
                EditorUtility.SetDirty(window.Config);
            }

            if (GUILayout.Button("Remove") && window.Config != null)
            {
                Undo.RecordObject(window.Config, "Remove Sprite Layer");
                window.Config.RemoveLayer(window.selectedLayer);
                window.selectedLayer = window.Config.LayerCount - 1;
                EditorUtility.SetDirty(window.Config);
            }

            EditorGUILayout.EndHorizontal();


            GUILayout.EndVertical();
        }

        private static void DrawLayerRow(SpriteEditorWindow window, int index)
        {
            Rect rowRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
            bool selected = window.selectedLayer == index;

            SpriteLayer layer = window.Config.GetLayer(index);
            if (layer == null) return;

            // Draw layer rect
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

            GUIContent eyeIcon = EditorGUIUtility.IconContent(
                                    layer.visible
                                        ? "animationvisibilitytoggleon"
                                        : "animationvisibilitytoggleoff");

            if (GUI.Button(eyeRect, eyeIcon, EditorStyles.iconButton))
                {
                    Undo.RecordObject(window.Config, "Toggle Layer Visibility");

                    layer.visible = !layer.visible;

                    EditorUtility.SetDirty(window.Config);
                    window.Repaint();
                }

            GUI.Label(
                    new Rect(
                        labelRect.x + 4f,
                        labelRect.y,
                        labelRect.width - 8f,
                        labelRect.height),
                        layer.name);

            if (index > 0 && GUI.Button(upRect, EditorGUIUtility.IconContent("scrollup"), EditorStyles.iconButton))
            {
                window.Config.MoveUp(index);
                window.selectedLayer--;
                EditorUtility.SetDirty(window.Config);
                window.Repaint();
            }

            if (index < window.Config.LayerCount - 1 && GUI.Button(downRect, EditorGUIUtility.IconContent("scrolldown"), EditorStyles.iconButton))
            {
                window.Config.MoveDown(index);
                window.selectedLayer++;
                EditorUtility.SetDirty(window.Config);
                window.Repaint();
            }

            if (Event.current.type == EventType.MouseDown &&
                rowRect.Contains(Event.current.mousePosition))
            {
                window.selectedLayer = index;
                Event.current.Use();
                window.Repaint();
            }
        }

        private static void DrawStates(SpriteEditorWindow window, float height)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(height));
            GUILayout.Label("State", EditorStyles.boldLabel);
            stateScroll = EditorGUILayout.BeginScrollView(stateScroll);
            EditorGUILayout.EndScrollView();


            if (GUILayout.Button("Create") && window.Config != null)
            {
                //Undo.RecordObject(window.Config, "Create Sprite State");
                //window.Config.AddLayer(new SpriteLayer());
                //selectedLayer = window.Config.LayerCount - 1;
                //EditorUtility.SetDirty(window.Config);
            }
            EditorGUILayout.EndVertical();
        }
    }
}
