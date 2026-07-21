using Haztech.SpriteEditor.Data;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Haztech.SpriteEditor.Editor
{
    public static class DisplayPanel
    {
        public static void Draw()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Preview", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();

            Rect previewRect = GUILayoutUtility.GetRect(300f, 300f, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            EditorGUI.DrawRect(
            previewRect,
            new Color(0.15f, 0.15f, 0.15f));

            Vector2 canvasSize = GetCanvasSize();

            DrawLayers(previewRect, canvasSize);
            EditorGUILayout.EndHorizontal();


            //EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(200));
            //GUILayout.Label("Details", EditorStyles.boldLabel);

            //DrawDetails(ToolWindow.Instance);

            //EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
        }
         
        private static Vector2 GetCanvasSize()
        {
            Vector2 size = Vector2.zero;

            SpriteConfig config = Window.Instance.SpriteConfig;

            if (config != null)
            {
                List<Layer> layers = config.GetLayers();
                for (int i = 0; i < layers.Count; i++)
                {
                    Layer layer = layers[i];
                    if (layer == null) continue;

                    StateData state = layer.GetState(Window.Instance.SpriteConfig.selectedState);
                    if (state == null) continue;

                    SpriteData data = state.GetData(Window.Instance.SpriteConfig.selectedDir);
                    if (data == null || data.sprite == null) continue;

                    size.x = Mathf.Max(size.x, data.sprite.rect.width);
                    size.y = Mathf.Max(size.y, data.sprite.rect.height);
                }
            }

            return size;
        }

        private static void DrawLayers(Rect previewRect, Vector2 canvasSize)
        {
            float scale = Mathf.Min(
            previewRect.width / canvasSize.x,
            previewRect.height / canvasSize.y);

            Vector2 displayedCanvasSize = canvasSize * scale;

            Rect canvasRect = new Rect(
                   previewRect.center.x - displayedCanvasSize.x * 0.5f,
                   previewRect.center.y - displayedCanvasSize.y * 0.5f,
                   displayedCanvasSize.x,
                   displayedCanvasSize.y);

            SpriteConfig config = Window.Instance.SpriteConfig;

            if (config != null)
            {
                for (int i = config.LayerCount; i >= 0; i--)
                {
                    LayerObject layerObj = config.GetLayerObj(i);

                    if (layerObj is LayerGroup group)
                    {
                        if (!group.visible) continue;

                        foreach(Layer layer in group.Layers)
                        {
                            if (!layer.visible) continue;
                            DrawLayer(layer, config, canvasRect, scale);
                        }
                    }
                    else if (layerObj is Layer layer)
                    {
                        if (!layer.visible) continue;
                        DrawLayer(layer, config, canvasRect, scale);
                    }             
                }
            }
        }

        private static void DrawLayer(Layer layer, SpriteConfig config, Rect rect, float scale)
        {
            StateData state = layer.GetState(Window.Instance.SpriteConfig.selectedState);
            if (state == null) return;

            SpriteData data = state.GetData(Window.Instance.SpriteConfig.selectedDir);
            if (data == null || data.sprite == null) return;

            ColorGroup colorGroup = null;

            if (layer.colorGroupId >= 0 && layer.colorGroupId < config.ColorGroups.Count)
                colorGroup = config.ColorGroups[layer.colorGroupId];

            DrawSprite(data.sprite, rect, scale, colorGroup == null ? layer.color : colorGroup.color);
        }

        private static void DrawSprite(Sprite sprite, Rect rect, float scale, Color color)
        {
            Texture2D texture = sprite.texture;

            Rect spriteRect = sprite.rect;

            Rect uvRect = new Rect(
                spriteRect.x / texture.width,
                spriteRect.y / texture.height,
                spriteRect.width / texture.width,
                spriteRect.height / texture.height);

            Rect drawRect = new Rect(
                rect.x,
                rect.y,
                spriteRect.width * scale,
                spriteRect.height * scale);

            Color previousColor = GUI.color;
            GUI.color = color;

            GUI.DrawTextureWithTexCoords(
                drawRect,
                texture,
                uvRect,
                true);

            GUI.color = previousColor;
        }

        static void DrawDetails()
        {
            Window.Instance.SpriteConfig.selectedDir = (Direction)EditorGUILayout.EnumPopup("Direction Mode", Window.Instance.SpriteConfig.selectedDir);
        }
    }
}
