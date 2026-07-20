using Haztech.SpriteEditor.Data;
using UnityEditor;
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

            if (config != null && window.SpriteConfig.selectedLayer >= 0 && window.SpriteConfig.selectedLayer < config.LayerCount)
            {
                Undo.RecordObject(config, "Edit Sprite Layer");

                Layer layer = config.GetLayer(window.SpriteConfig.selectedLayer);

                if (layer != null)
                {
                    layer.visible = EditorGUILayout.Toggle("Visible", layer.visible);
                    layer.name = EditorGUILayout.TextField("Name", layer.name);

                    DrawColor(window);

                    ColorGroup colorGroup = null;

                    if (layer.colorGroupId >= 0 && layer.colorGroupId < config.ColorGroups.Count) 
                        colorGroup = config.ColorGroups[layer.colorGroupId];

                    if (colorGroup == null)
                        layer.color = EditorGUILayout.ColorField("Color", layer.color);
                    else
                        colorGroup.color = EditorGUILayout.ColorField("Group Color", colorGroup.color);


                    StateData state = layer.GetState(window.SpriteConfig.selectedState);
                    if (state != null)
                    {
                        SpriteData data = state.GetData(window.SpriteConfig.selectedDir);
                        data.sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", data.sprite, typeof(Sprite), false);
                    }
                }

            }
            EditorGUILayout.EndVertical();
        }


        private static void DrawColor(ToolWindow window)
        {
            EditorGUILayout.BeginHorizontal();

            SpriteConfig config = window.SpriteConfig;

            if (config != null)
            {
                Layer layer = config.GetLayer(window.SpriteConfig.selectedLayer);

                if (layer != null)
                {
                    string[] options = new string[config.ColorGroups.Count + 1];
                    options[0] = "None";

                    for (int i = 0; i < config.ColorGroups.Count; i++)
                    {
                        options[i + 1] = (i + 1).ToString() + ": " + config.ColorGroups[i].name;
                    }

                    layer.colorGroupId = EditorGUILayout.Popup(
                                        layer.colorGroupId + 1,
                                        options
                                    ) - 1;

                    if (GUILayout.Button("+", GUILayout.Width(EditorGUIUtility.singleLineHeight), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                    {
                        config.ColorGroups.Add(new ColorGroup());
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        private static void DrawStateProperties(ToolWindow window, float height)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(height));
            GUILayout.Label("State " +
                "Properties", EditorStyles.boldLabel);

            SpriteConfig config = window.SpriteConfig;

            if (config != null && window.SpriteConfig.selectedLayer >= 0 && window.SpriteConfig.selectedLayer < config.LayerCount)
            {
                Undo.RecordObject(config, "Edit Sprite State");

                StateConfig state = config.GetStateConfig(window.SpriteConfig.selectedState);

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
