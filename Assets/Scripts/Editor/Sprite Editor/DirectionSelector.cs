using Haztech.SpriteEditor.Data;
using UnityEditor;
using UnityEngine;
namespace Haztech.SpriteEditor.Editor
{
    public static class DirectionSelector
    {
        public static float Draw(ToolWindow window)
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
            DrawDirButton(window, "←", Direction.West);
            DrawDirButton(window, "", Direction.Null);
            DrawDirButton(window, "→", Direction.East);
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
                Color old = GUI.backgroundColor;

                SpriteConfig config = window.SpriteConfig;

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