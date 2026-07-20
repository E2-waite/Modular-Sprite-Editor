using UnityEditor;
using UnityEngine;
using Haztech.SpriteEditor.Data;

namespace Haztech.SpriteEditor.Editor
{
    public static class DisplayPanel
    {
        public static void Draw(ToolWindow window)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Preview", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();

            Rect previewRect = GUILayoutUtility.GetRect(300f, 300f, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            EditorGUI.DrawRect(
            previewRect,
            new Color(0.15f, 0.15f, 0.15f));

            Vector2 canvasSize = GetCanvasSize(window);

            DrawLayers(window, previewRect, canvasSize);
            EditorGUILayout.EndHorizontal();


            //EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(200));
            //GUILayout.Label("Details", EditorStyles.boldLabel);

            //DrawDetails(window);

            //EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
        }
         
        private static Vector2 GetCanvasSize(ToolWindow window)
        {
            Vector2 size = Vector2.zero;

            SpriteConfig config = window.SpriteConfig;

            if (config != null)
            {
                for (int i = 0; i < config.LayerCount; i++)
                {
                    Layer layer = config.GetLayer(i);
                    if (layer == null) continue;

                    StateData state = layer.GetState(window.SpriteConfig.selectedState);
                    if (state == null) continue;

                    SpriteData data = state.GetData(window.SpriteConfig.selectedDir);
                    if (data == null || data.sprite == null) continue;

                    size.x = Mathf.Max(size.x, data.sprite.rect.width);
                    size.y = Mathf.Max(size.y, data.sprite.rect.height);
                }
            }

            return size;
        }

        private static void DrawLayers(ToolWindow window, Rect previewRect, Vector2 canvasSize)
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

            SpriteConfig config = window.SpriteConfig;

            if (config != null)
            {
                for (int i = config.ExpandedLayers.Count - 1; i >= 0; i--)
                {
                    LayerObject layerObj = config.ExpandedLayers[i];

                    if (layerObj is LayerGroup) continue;

                    Layer layer = (Layer)layerObj;
                    if (layer == null || !layer.visible) continue;

                    StateData state = layer.GetState(window.SpriteConfig.selectedState);
                    if (state == null) continue;

                    SpriteData data = state.GetData(window.SpriteConfig.selectedDir);
                    if (data == null || data.sprite == null) continue;

                    ColorGroup colorGroup = null;

                    if (layer.colorGroupId >= 0 && layer.colorGroupId < config.ColorGroups.Count)
                        colorGroup = config.ColorGroups[layer.colorGroupId];

                    DrawSprite(data.sprite, canvasRect, scale, colorGroup == null ? layer.color : colorGroup.color);
                }
            }
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

        static void DrawDetails(ToolWindow window)
        {
            window.SpriteConfig.selectedDir = (Direction)EditorGUILayout.EnumPopup("Direction Mode", window.SpriteConfig.selectedDir);
        }
    }
}
