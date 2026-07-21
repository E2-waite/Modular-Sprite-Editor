using UnityEditor;
using UnityEngine;

namespace Haztech.SpriteEditor.Editor
{
    public static class ToolbarFileMenu
    {
        private static string lastDir = Application.dataPath;

        public static void Draw()
        {
            Rect fileRect = GUILayoutUtility.GetRect(
                new GUIContent("File"),
                EditorStyles.toolbarDropDown,
                GUILayout.Width(50));

            if (EditorGUI.DropdownButton(
                fileRect,
                new GUIContent("File"),
                FocusType.Passive,
                EditorStyles.toolbarDropDown))
            {
                DrawFileMenu(fileRect);
            }
        }

        private static void DrawFileMenu(Rect buttonRect)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(
                new GUIContent("New..."),
                false,
                () => New());

            menu.AddItem(
                new GUIContent("Open..."),
                false,
                () => Open());

            menu.DropDown(buttonRect);
        }


        private static void New()
        {
            Debug.Log("New");

            string path = EditorUtility.SaveFilePanelInProject(
                "Create Sprite Configuration",
                "New Sprite Configuration",
                "asset",
                "Choose where to save the configuration.",
                lastDir);

            if (string.IsNullOrEmpty(path))
                return;

            lastDir = path;

            Window.Instance.NewConfig(path);
        }


        private static void Open()
        {
            string path = EditorUtility.OpenFilePanel(
                            "Open Sprite Configuration",
                            lastDir,
                            "asset");

            if (string.IsNullOrEmpty(path))
                return;

            lastDir = path;

            Window.Instance.OpenConfig("Assets" + path.Substring(Application.dataPath.Length));
        }
    }
}