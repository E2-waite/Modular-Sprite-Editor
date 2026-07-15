using UnityEditor;
using UnityEngine;
using Haztech.SpriteEditor.Data;

namespace Haztech.SpriteEditor.Editor
{
    public static class PropertiesPanel
    {
        public static void Draw(ToolWindow window)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(260));

            GUILayout.Label("Properties", EditorStyles.boldLabel);

            SpriteConfig config = window.SpriteConfig;

            if (config != null && window.selectedLayer >= 0 && window.selectedLayer < config.LayerCount)
            {
                Undo.RecordObject(config, "Edit Sprite Layer");

                Layer layer = config.GetLayer(window.selectedLayer);

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