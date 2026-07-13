using UnityEditor;
using UnityEngine;
using SpriteEditor.Data;

namespace SpriteEditor.Editor
{
    public class SpriteEditorWindow : EditorWindow
    {
        [SerializeField] private SpriteEditorConfig config;
        public SpriteEditorConfig Config => config;
        [SerializeField] public int selectedLayer = -1;

        [MenuItem("Tools/Sprite Editor")]
        public static void ShowWindow()
        {
            GetWindow<SpriteEditorWindow>("Sprite Editor");
        }

        void OnGUI()
        {
            SpriteEditorToolbar.Draw(this);

            EditorGUILayout.BeginHorizontal();

            SpriteEditorLayerPanel.Draw(this);
            SpriteEditorDisplayPanel.Draw(this);
            SpriteEditorPropertiesPanel.Draw(this);
            EditorGUILayout.EndHorizontal();
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
}