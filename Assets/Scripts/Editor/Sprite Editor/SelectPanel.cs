using Haztech.SpriteEditor.Data;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Haztech.SpriteEditor.Editor
{
    public static class SelectPanel
    {
        public static void Draw()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(220),
                 GUILayout.ExpandHeight(true));

            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float availableHeight = ToolWindow.Instance.position.height - 160;
            availableHeight -= DirectionSelector.Draw();


            float sectionHeight = (availableHeight - spacing) * .5f;

            GUILayout.Space(spacing);

            LayerList.Draw(sectionHeight);

            GUILayout.Space(spacing);

            StateList.Draw(sectionHeight);

            EditorGUILayout.EndVertical();
        }
    }
}
