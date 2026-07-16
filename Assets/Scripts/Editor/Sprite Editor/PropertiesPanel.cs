using Haztech.SpriteEditor.Data;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace Haztech.SpriteEditor.Editor
{
    public static class PropertiesPanel
    {
        public static void Draw(ToolWindow window)
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(260));
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float availableHeight = window.position.height - 25f;
            float sectionHeight = (availableHeight - spacing) * 0.5f;
            DrawLayerProperties(window, sectionHeight);
            DrawStateProperties(window, sectionHeight);

            EditorGUILayout.EndVertical();
        }


        private static void DrawLayerProperties(ToolWindow window, float height)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(height));
            GUILayout.Label("Layer " +
                "Properties", EditorStyles.boldLabel);

            SpriteConfig config = window.SpriteConfig;

            if (config != null && window.selectedLayer >= 0 && window.selectedLayer < config.LayerCount)
            {
                Undo.RecordObject(config, "Edit Sprite Layer");

                Layer layer = config.GetLayer(window.selectedLayer);

                if (layer != null)
                {
                    layer.visible = EditorGUILayout.Toggle("Visible", layer.visible);
                    layer.name = EditorGUILayout.TextField("Name", layer.name);
                    layer.color = EditorGUILayout.ColorField("Color", layer.color);



                    StateData state = layer.GetState(window.selectedState);
                    if (state != null)
                    {
                        SpriteData data = state.GetData(window.selectedDir);
                        data.sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", data.sprite, typeof(Sprite), false);
                    }
                }

                EditorUtility.SetDirty(config);
            }
            EditorGUILayout.EndVertical();
        }

        private static void DrawStateProperties(ToolWindow window, float height)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(height));
            GUILayout.Label("State " +
                "Properties", EditorStyles.boldLabel);

            SpriteConfig config = window.SpriteConfig;

            if (config != null && window.selectedLayer >= 0 && window.selectedLayer < config.LayerCount)
            {
                Undo.RecordObject(config, "Edit Sprite State");

                StateConfig state = config.GetStateConfig(window.selectedState);

                if (state != null)
                {
                    state.name = EditorGUILayout.TextField("Name", state.name);
                }

                EditorUtility.SetDirty(config);
            }
            EditorGUILayout.EndVertical();
        }


    }
}
