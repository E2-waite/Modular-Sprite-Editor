using UnityEditor;
using UnityEngine;
using SpriteEditor.Data;

namespace SpriteEditor.Editor
{
    public class SpriteEditorWindow : EditorWindow
    {
        [SerializeField] private SpriteEditorConfig config;
        const string LastConfigKey = "SpriteEditor.LastConfig";

        public SpriteEditorConfig Config => config;
        [SerializeField] public int selectedLayer = -1;

        private void OnEnable()
        {
            string path = EditorPrefs.GetString(LastConfigKey, "");
            if (!string.IsNullOrEmpty(path))
            {
                config = AssetDatabase.LoadAssetAtPath<SpriteEditorConfig>(path);
            }
        }

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

            // Sets previous config to ensure the same config opens after closing and re-opening
            EditorPrefs.SetString(LastConfigKey, path);

            AssetDatabase.CreateAsset(newConfig, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            config = newConfig;
        }

        public void OpenConfig(string path)
        {
            EditorPrefs.SetString(LastConfigKey, path);
            config = AssetDatabase.LoadAssetAtPath<SpriteEditorConfig>(path);
        }
    }
}