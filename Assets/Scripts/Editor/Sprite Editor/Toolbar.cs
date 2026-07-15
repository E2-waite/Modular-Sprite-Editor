using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace Haztech.SpriteEditor.Editor
{
    public class Toolbar
    {
        private static string lastDir = Application.dataPath;

        public static void Draw(ToolWindow editor)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

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
                DrawFileMenu(fileRect, editor);
            }

            GUILayout.FlexibleSpace();

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();
        }

        static void DrawFileMenu(Rect buttonRect, ToolWindow editor)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(
                new GUIContent("New..."),
                false,
                () => New(editor));

            menu.AddItem(
                new GUIContent("Open..."),
                false,
                () => Open(editor));

            menu.DropDown(buttonRect);
        }


        static void New(ToolWindow editor)
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

            editor.NewConfig(path);
        }


        static void Open(ToolWindow editor)
        {
            string path = EditorUtility.OpenFilePanel(
                            "Open Sprite Configuration",
                            lastDir,
                            "asset");

            if (string.IsNullOrEmpty(path))
                return;

            lastDir = path;

            editor.OpenConfig("Assets" + path.Substring(Application.dataPath.Length));
        }
    }
}