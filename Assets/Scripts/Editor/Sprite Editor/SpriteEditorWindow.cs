using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using static SpriteEditorToolbar;

public class SpriteEditorWindow : EditorWindow
{
    SpriteEditorConfig config;
    private int selectedLayer = -1;
    private Vector2 layerScroll;

    [MenuItem("Tools/Sprite Editor")]
    public static void ShowWindow()
    {
        GetWindow<SpriteEditorWindow>("Sprite Editor");
    }

    void OnGUI()
    {
        SpriteEditorToolbar.Draw(this);

        EditorGUILayout.BeginHorizontal();

        DrawLayerList();
        DrawDisplayPanel();
        DrawLayerProperties();

        EditorGUILayout.EndHorizontal();
    }

    private void DrawTopBar()
    {

    }

    private void DrawLayerList()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(160));

        GUILayout.Label("Layers", EditorStyles.boldLabel);

        layerScroll = EditorGUILayout.BeginScrollView(layerScroll);

        if (config != null && config.LayerCount > 0)
        {
            for (int i = 0; i < config.LayerCount; i++)
            {
                SpriteLayer layer = config.GetLayer(i);
                if (layer == null) continue;

                GUI.backgroundColor =
                selectedLayer == i
                    ? new Color(0.3f, 0.6f, 1f)
                    : Color.white;

                if (GUILayout.Button(layer.name))
                {
                    selectedLayer = i;
                }
            }
            GUI.backgroundColor = Color.white;
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        if (GUILayout.Button("Create") && config != null)
        {
            Undo.RecordObject(config, "Create Sprite Layer");
            config.AddLayer(new SpriteLayer());
            selectedLayer = config.LayerCount - 1;
            EditorUtility.SetDirty(config);
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawDisplayPanel()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label("Preview", EditorStyles.boldLabel);

        Rect previewRect = GUILayoutUtility.GetRect(
                                            300f,
                                            300f,
                                            GUILayout.ExpandWidth(true),
                                            GUILayout.ExpandHeight(true));

        EditorGUI.DrawRect(
        previewRect,
        new Color(0.15f, 0.15f, 0.15f));

        Vector2 canvasSize = GetCanvasSize();

        DrawLayers(previewRect, canvasSize);

        EditorGUILayout.EndVertical();

    }

    private Vector2 GetCanvasSize()
    {
        Vector2 size = Vector2.zero;

        if (config != null)
        {
            foreach (SpriteLayer layer in config.Layers)
            {
                if (layer == null || layer.sprite == null) continue;

                size.x = Mathf.Max(size.x, layer.sprite.rect.width);
                size.y = Mathf.Max(size.x, layer.sprite.rect.height);
            }
        }
        
        return size;
    }

    private void DrawLayerProperties()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(160));

        GUILayout.Label("Params", EditorStyles.boldLabel);

        if (config != null && selectedLayer >= 0 && selectedLayer < config.LayerCount)
        {
            Undo.RecordObject(config, "Edit Sprite Layer");

            SpriteLayer layer = config.GetLayer(selectedLayer);

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

    private void DrawLayers(Rect previewRect, Vector2 canvasSize)
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
            foreach (SpriteLayer layer in config.Layers)
            {
                if (layer == null || layer.sprite == null || !layer.visible) continue;

                DrawSprite(layer.sprite, canvasRect, scale, layer.color);
            }
        }
    }

    private void DrawSprite(Sprite sprite, Rect rect, float scale, Color color)
    {
        Texture2D texture = sprite.texture;

        // The actual portion stored in the texture.
        Rect textureRect = sprite.textureRect;

        // Position of the trimmed texture inside the sprite's original rectangle.
        Vector2 offset = sprite.textureRectOffset;

        Rect uvRect = new Rect(
            textureRect.x / texture.width,
            textureRect.y / texture.height,
            textureRect.width / texture.width,
            textureRect.height / texture.height);

        Rect drawRect = new Rect(
            rect.x + offset.x * scale,

            // Convert Unity's bottom-up sprite coordinates into GUI's top-down coordinates.
            rect.y +
            (sprite.rect.height - offset.y - textureRect.height) * scale,

            textureRect.width * scale,
            textureRect.height * scale);

        Color previousColor = GUI.color;
        GUI.color = color;

        GUI.DrawTextureWithTexCoords(
            drawRect,
            texture,
            uvRect,
            true);

        GUI.color = previousColor;
    }

    public void NewConfig(string path)
    {
        SpriteEditorConfig newConfig = ScriptableObject.CreateInstance<SpriteEditorConfig>();

        AssetDatabase.CreateAsset(newConfig, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        config = newConfig;
    }

    public void OpenConfig(string path)
    {
        config = AssetDatabase.LoadAssetAtPath<SpriteEditorConfig>(path);
    }
}
