using Haztech.SpriteEditor.Data;
using UnityEditor;
using UnityEngine;

namespace Haztech.SpriteEditor.Editor
{
    public static class SelectPanel
    {
        [SerializeField] private static Vector2 layerScroll;
        [SerializeField] private static Vector2 stateScroll;

        public static void Draw(ToolWindow window)
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(220),
                 GUILayout.ExpandHeight(true));

            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float availableHeight = window.position.height - 160;
            availableHeight -= DrawDirectionSelector(window);


            float sectionHeight = (availableHeight - spacing) * .5f;

            GUILayout.Space(spacing);


            DrawLayersList(window, sectionHeight);

            GUILayout.Space(spacing);

            DrawStates(window, sectionHeight);

            EditorGUILayout.EndVertical();
        }

        private static float DrawDirectionSelector(ToolWindow window)
        {
            Rect sectionRect = EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Directions", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            DrawDirButton(window, "↖", Direction.NorthWest);
            DrawDirButton(window, "↑", Direction.North);
            DrawDirButton(window, "↗", Direction.NorthEast);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            DrawDirButton(window, "←", Direction.SouthWest);
            DrawDirButton(window, "", Direction.Null);
            DrawDirButton(window, "→", Direction.SouthEast);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            DrawDirButton(window, "↙", Direction.SouthWest);
            DrawDirButton(window, "↓", Direction.South);
            DrawDirButton(window, "↘", Direction.SouthEast);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            return sectionRect.height;
        }

        private static void DrawDirButton(ToolWindow window, string icon, Direction dir)
        {
            if (dir == Direction.Null)
            {
                GUILayout.Space(34);
            }
            else
            {
                if (GUILayout.Button(icon, GUILayout.Width(32), GUILayout.Height(32)))
                {
                    window.SpriteConfig.selectedDir = dir;
                }
            }
        }

        private static void DrawLayersList(ToolWindow window, float height)
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

            if (window.SpriteConfig != null && window.SpriteConfig.LayerCount > 0)
            {
                for (int i = 0; i < window.SpriteConfig.LayerCount; i++)
                {
                    DrawLayerRow(window, i);
                }
                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Create") && window.SpriteConfig != null)
            {
                Undo.RecordObject(window.SpriteConfig, "Create Sprite Layer");
                window.SpriteConfig.AddLayer(new Layer("New Layer"));
                window.SpriteConfig.selectedLayer = window.SpriteConfig.LayerCount - 1;
                EditorUtility.SetDirty(window.SpriteConfig);
            }

            using (new EditorGUI.DisabledScope(window.SpriteConfig.LayerCount <= 1))
            {
                if (GUILayout.Button("Remove") && window.SpriteConfig != null)
                {
                    Undo.RecordObject(window.SpriteConfig, "Remove Sprite Layer");
                    window.SpriteConfig.RemoveLayer(window.SpriteConfig.selectedLayer);
                    window.SpriteConfig.selectedLayer = window.SpriteConfig.LayerCount - 1;
                    EditorUtility.SetDirty(window.SpriteConfig);
                }
            }


            EditorGUILayout.EndHorizontal();


            GUILayout.EndVertical();
        }

        private static void DrawLayerRow(ToolWindow window, int index)
        {
            Rect rowRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
            bool selected = window.SpriteConfig.selectedLayer == index;

            Layer layer = window.SpriteConfig.GetLayer(index);
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
                    Undo.RecordObject(window.SpriteConfig, "Toggle Layer Visibility");

                    layer.visible = !layer.visible;

                    EditorUtility.SetDirty(window.SpriteConfig);
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
                window.SpriteConfig.MoveLayerUp(index);
                window.SpriteConfig.selectedLayer--;
                EditorUtility.SetDirty(window.SpriteConfig);
                window.Repaint();
            }

            if (index < window.SpriteConfig.LayerCount - 1 && GUI.Button(downRect, EditorGUIUtility.IconContent("scrolldown"), EditorStyles.iconButton))
            {
                window.SpriteConfig.MoveLayerDown(index);
                window.SpriteConfig.selectedLayer++;
                EditorUtility.SetDirty(window.SpriteConfig);
                window.Repaint();
            }

            if (Event.current.type == EventType.MouseDown &&
                rowRect.Contains(Event.current.mousePosition))
            {
                window.SpriteConfig.selectedLayer = index;
                Event.current.Use();
                window.Repaint();
            }
        }

        private static void DrawStates(ToolWindow window, float height)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(height));
            GUILayout.Label("States", EditorStyles.boldLabel);


            Rect listRect = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(listRect, new Color(0.2f, 0.2f, 0.2f));

            stateScroll = EditorGUILayout.BeginScrollView(stateScroll);

            //if (Event.current.type == EventType.MouseDown &&
            //            listRect.Contains(Event.current.mousePosition))
            //{
            //    window.selectedState = -1;
            //    Event.current.Use();
            //    window.Repaint();
            //}

            if (window.SpriteConfig != null && window.SpriteConfig.StateCount > 0)
            {
                for (int i = 0; i < window.SpriteConfig.StateCount; i++)
                {
                    DrawStateRow(window, i);
                }
                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Create") && window.SpriteConfig != null)
            {
                Undo.RecordObject(window.SpriteConfig, "Create Sprite State");
                window.SpriteConfig.AddState(new StateConfig("New State"));
                window.SpriteConfig.selectedState = window.SpriteConfig.StateCount - 1;
                EditorUtility.SetDirty(window.SpriteConfig);
            }

            using (new EditorGUI.DisabledScope(window.SpriteConfig.StateCount <= 1))
            {
                if (GUILayout.Button("Remove") && window.SpriteConfig != null)
                {
                    Undo.RecordObject(window.SpriteConfig, "Remove Sprite State");
                    window.SpriteConfig.RemoveState(window.SpriteConfig.selectedState);
                    window.SpriteConfig.selectedState = window.SpriteConfig.StateCount - 1;
                    EditorUtility.SetDirty(window.SpriteConfig);
                }
            }
            EditorGUILayout.EndHorizontal();


            GUILayout.EndVertical();
        }

        private static void DrawStateRow(ToolWindow window, int index)
        {
            Rect rowRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
            bool selected = window.SpriteConfig.selectedState == index;

            StateConfig state = window.SpriteConfig.GetStateConfig(index);
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
                window.SpriteConfig.MoveLayerUp(index);
                window.SpriteConfig.selectedState--;
                EditorUtility.SetDirty(window.SpriteConfig);
                window.Repaint();
            }

            if (index < window.SpriteConfig.StateCount - 1 && GUI.Button(downRect, EditorGUIUtility.IconContent("scrolldown"), EditorStyles.iconButton))
            {
                window.SpriteConfig.MoveLayerDown(index);
                window.SpriteConfig.selectedState++;
                EditorUtility.SetDirty(window.SpriteConfig);
                window.Repaint();
            }

            if (Event.current.type == EventType.MouseDown &&
                rowRect.Contains(Event.current.mousePosition))
            {
                window.SpriteConfig.selectedState = index;
                Event.current.Use();
                window.Repaint();
            }
        }
    }
}
