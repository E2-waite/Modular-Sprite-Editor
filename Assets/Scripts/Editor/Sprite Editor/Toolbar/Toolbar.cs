using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace Haztech.SpriteEditor.Editor
{
    public static class Toolbar
    {
        public static void Draw()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            ToolbarFileMenu.Draw();
            ToolbarEditMenu.Draw();

            EditorGUILayout.EndHorizontal();
        }
    }
}