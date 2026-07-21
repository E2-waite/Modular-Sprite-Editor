using Haztech.SpriteEditor.Data;
using UnityEditor;
using UnityEngine;

namespace Haztech.SpriteEditor.Editor
{
    public static class StateRow
    {
        public static void Draw(int index)
        {
            SpriteConfig config = Window.Instance.SpriteConfig;

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
                Window.Instance.Repaint();
            }

            if (index < config.StateCount - 1 && GUI.Button(downRect, EditorGUIUtility.IconContent("scrolldown"), EditorStyles.iconButton))
            {
                config.MoveLayerDown(index);
                config.selectedState++;
                EditorUtility.SetDirty(config);
                Window.Instance.Repaint();
            }

            if (Event.current.type == EventType.MouseDown &&
                rowRect.Contains(Event.current.mousePosition))
            {
                config.selectedState = index;
                Event.current.Use();
                Window.Instance.Repaint();
            }
        }
    }
}