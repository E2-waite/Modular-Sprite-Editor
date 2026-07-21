using Haztech.SpriteEditor.Data;
using UnityEditor;
using UnityEngine;

namespace Haztech.SpriteEditor.Editor
{
    public static class PropertiesPanel
    {
        public static void Draw()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(260));
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float availableHeight = Window.Instance.position.height - 25f;
            float sectionHeight = (availableHeight - spacing) * 0.5f;
            DrawLayerProperties(sectionHeight);
            DrawStateProperties(sectionHeight);

            EditorGUILayout.EndVertical();
        }


        private static void DrawLayerProperties(float height)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(height));
            GUILayout.Label("Layer Properties", EditorStyles.boldLabel);

            SpriteConfig config = Window.Instance.SpriteConfig;

            if (config != null && Window.Instance.SpriteConfig.selectedLayer >= 0 && Window.Instance.SpriteConfig.selectedLayer < config.ExpandedLayers.Count)
            {
                LayerObject layerObj = config.ExpandedLayers[Window.Instance.SpriteConfig.selectedLayer];

                if (layerObj is Layer layer)
                {
                    Undo.RecordObject(config, "Edit Layer");

                    layer.visible = EditorGUILayout.Toggle("Visible", layer.visible);
                    layer.name = EditorGUILayout.TextField("Name", layer.name);

                    DrawColor();

                    ColorGroup colorGroup = null;

                    if (layer.colorGroupId >= 0 && layer.colorGroupId < config.ColorGroups.Count) 
                        colorGroup = config.ColorGroups[layer.colorGroupId];

                    if (colorGroup == null)
                        layer.color = EditorGUILayout.ColorField("Color", layer.color);
                    else
                        colorGroup.color = EditorGUILayout.ColorField("Group Color", colorGroup.color);


                    StateData state = layer.GetState(Window.Instance.SpriteConfig.selectedState);
                    if (state != null)
                    {
                        SpriteData data = state.GetData(Window.Instance.SpriteConfig.selectedDir);
                        data.sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", data.sprite, typeof(Sprite), false);
                    }
                }
                else if (layerObj is LayerGroup group)
                {
                    Undo.RecordObject(config, "Edit Layer Group");
                    group.visible = EditorGUILayout.Toggle("Visible", group.visible);
                    group.name = EditorGUILayout.TextField("Name", group.name);

                    GUILayout.Label(group.Layers.Count + " Layers");
                }

            }
            EditorGUILayout.EndVertical();
        }


        private static void DrawColor()
        {
            EditorGUILayout.BeginHorizontal();

            SpriteConfig config = Window.Instance.SpriteConfig;

            if (config != null)
            {
                LayerObject layerObj = config.ExpandedLayers[Window.Instance.SpriteConfig.selectedLayer];

                if (layerObj != null && layerObj is Layer layer)
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
        private static void DrawStateProperties(float height)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(height));
            GUILayout.Label("State " +
                "Properties", EditorStyles.boldLabel);

            SpriteConfig config = Window.Instance.SpriteConfig;

            if (config != null && Window.Instance.SpriteConfig.selectedLayer >= 0 && Window.Instance.SpriteConfig.selectedLayer < config.LayerCount)
            {
                Undo.RecordObject(config, "Edit Sprite State");

                StateConfig state = config.GetStateConfig(Window.Instance.SpriteConfig.selectedState);

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
