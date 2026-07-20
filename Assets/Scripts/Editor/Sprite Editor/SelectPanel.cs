using Haztech.SpriteEditor.Data;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Haztech.SpriteEditor.Editor
{
    public static class SelectPanel
    {
        public static void Draw(ToolWindow window)
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(220),
                 GUILayout.ExpandHeight(true));

            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float availableHeight = window.position.height - 160;
            availableHeight -= DirectionSelector.Draw(window);


            float sectionHeight = (availableHeight - spacing) * .5f;

            GUILayout.Space(spacing);

            LayerList.Draw(window, sectionHeight);

            GUILayout.Space(spacing);

            StateList.Draw(window, sectionHeight);

            EditorGUILayout.EndVertical();
        }

        
    }
}
