using UnityEditor;
using UnityEngine;

namespace Haztech.SpriteEditor.Editor
{
    public static class ToolbarEditMenu
    {
        public static void Draw()
        {
            Rect editRect = GUILayoutUtility.GetRect(
                new GUIContent("Edit"),
                EditorStyles.toolbarDropDown,
                GUILayout.Width(50));

            if (EditorGUI.DropdownButton(
                editRect,
                new GUIContent("Edit"),
                FocusType.Passive,
                EditorStyles.toolbarDropDown))
            {
                DrawEditMenu(editRect);
            }
        }

        private static void DrawEditMenu(Rect buttonRect)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(
                new GUIContent("Settings"),
                false,
                () => Settings());

            menu.DropDown(buttonRect);
        }

        private static void Settings()
        {

        }
    }
}