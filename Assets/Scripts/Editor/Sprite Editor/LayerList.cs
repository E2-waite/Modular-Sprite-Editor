using Haztech.SpriteEditor.Data;
using Haztech.SpriteEditor.Editor;
using UnityEditor;
using UnityEngine;

namespace Haztech.SpriteEditor.Editor
{
    public static class LayerList
    {
        [SerializeField] private static Vector2 scroll;

        public static void Draw(ToolWindow window, float height)
        {
            SpriteConfig config = window.SpriteConfig;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(height));
            GUILayout.Label("Layers", EditorStyles.boldLabel);

            Rect listRect = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(listRect, new Color(0.2f, 0.2f, 0.2f));

            scroll = EditorGUILayout.BeginScrollView(scroll);

            if (config != null && config.ExpandedLayers.Count > 0)
            {
                for (int i = 0; i < config.ExpandedLayers.Count; i++)
                {
                    LayerRow.Draw(window, i);
                }
                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("New Layer") && config != null)
            {
                Undo.RecordObject(config, "Create Sprite Layer");
                config.AddLayer(new Layer("New Layer", config));
                config.selectedLayer = config.ExpandedLayers.Count - 1;
                EditorUtility.SetDirty(config);
            }

            if (GUILayout.Button("New Group") && config != null)
            {
                Undo.RecordObject(config, "Create Sprite Group");
                config.AddGroup(new LayerGroup("New Group", config));
                config.selectedLayer = config.ExpandedLayers.Count - 1;
                EditorUtility.SetDirty(config);
            }

            using (new EditorGUI.DisabledScope(config.ExpandedLayers.Count <= 1))
            {
                if (GUILayout.Button("Remove") && config != null)
                {
                    Undo.RecordObject(config, "Remove");
                    config.RemoveLayer(config.ExpandedLayers[config.selectedLayer]);
                    config.selectedLayer = config.ExpandedLayers.Count - 1;
                    EditorUtility.SetDirty(config);
                }
            }

            EditorGUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}