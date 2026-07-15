using UnityEditor;
using UnityEngine;
using Haztech.SpriteEditor.Data;

namespace Haztech.SpriteEditor.Editor
{
    public static class DisplayPanel
    {
        public static void Draw(ToolWindow window)
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Label("Preview", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

            Rect previewRect = GUILayoutUtility.GetRect(
                                                300f,
                                                300f,
                                                GUILayout.ExpandWidth(true),
                                                GUILayout.ExpandHeight(true));

            EditorGUI.DrawRect(
            previewRect,
            new Color(0.15f, 0.15f, 0.15f));

            Vector2 canvasSize = GetCanvasSize(window.SpriteConfig);

            DrawLayers(window.SpriteConfig, previewRect, canvasSize);
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Height(200));
            GUILayout.Label("Details", EditorStyles.boldLabel);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private static Vector2 GetCanvasSize(SpriteConfig config)
        {
            Vector2 size = Vector2.zero;

            if (config != null)
            {
                foreach (Layer layer in config.Layers)
                {
                    if (layer == null || layer.sprite == null) continue;

                    size.x = Mathf.Max(size.x, layer.sprite.rect.width);
                    size.y = Mathf.Max(size.y, layer.sprite.rect.height);
                }
            }

            return size;
        }

        private static void DrawLayers(SpriteConfig config, Rect previewRect, Vector2 canvasSize)
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

            if (config != null)
            {
                for (int i = config.LayerCount - 1; i >= 0; i--)
                {
                    Layer layer = config.GetLayer(i);
                    if (layer == null || layer.sprite == null || !layer.visible) continue;
                    DrawSprite(layer.sprite, canvasRect, scale, layer.color);
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
    }
}
