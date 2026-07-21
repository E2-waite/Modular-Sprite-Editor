using Haztech.SpriteEditor.Data;
using UnityEditor;
using UnityEngine;

namespace Haztech.SpriteEditor.Editor
{
    public static class DirectionSelector
    {
        public static float Draw()
        {
            Rect sectionRect = EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Directions", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical();

            DrawDirRow(("↖", Direction.NorthWest),  ("↑", Direction.North), ("↗", Direction.NorthEast));
            DrawDirRow(("←", Direction.West),       ("", Direction.Null),   ("→", Direction.East));
            DrawDirRow(("↙", Direction.SouthWest),  ("↓", Direction.South), ("↘", Direction.SouthEast));

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            return sectionRect.height;
        }

        private static void DrawDirRow(params (string icon, Direction dir)[] points)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            foreach (var point in points)
            {
                DrawDirButton(point.icon, point.dir);
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private static void DrawDirButton(string icon, Direction dir)
        {
            if (dir == Direction.Null)
            {
                GUILayout.Space(34);
            }
            else
            {
                Color old = GUI.backgroundColor;

                SpriteConfig config = Window.Instance.SpriteConfig;

                if (config.selectedDir == dir)
                {
                    GUI.backgroundColor = new Color(0.35f, 0.6f, 1f);
                }

                if (GUILayout.Button(icon, GUILayout.Width(32), GUILayout.Height(32)))
                {
                    config.selectedDir = dir;
                }

                GUI.backgroundColor = old;
            }
        }
    }
}