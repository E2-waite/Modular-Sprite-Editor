using UnityEditor;
using UnityEngine;
using SpriteEditor.Data;

namespace SpriteEditor.Editor
{
    public static class SpriteEditorPropertiesPanel
    {
        public static void Draw(SpriteEditorWindow window)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(260));

            GUILayout.Label("Properties", EditorStyles.boldLabel);

            SpriteEditorConfig config = window.Config;

            if (config != null && window.selectedLayer >= 0 && window.selectedLayer < config.LayerCount)
            {
                Undo.RecordObject(config, "Edit Sprite Layer");

                SpriteLayer layer = config.GetLayer(window.selectedLayer);

                if (layer != null)
                {
                    layer.visible = EditorGUILayout.Toggle("Visible", layer.visible);
                    layer.name = EditorGUILayout.TextField("Name", layer.name);
                    layer.sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", layer.sprite, typeof(Sprite), false);
                    layer.color = EditorGUILayout.ColorField("Color", layer.color);
                }

                EditorUtility.SetDirty(config);
            }

            EditorGUILayout.EndVertical();
        }
    }
}