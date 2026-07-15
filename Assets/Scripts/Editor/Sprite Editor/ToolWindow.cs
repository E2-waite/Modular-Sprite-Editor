using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Haztech.SpriteEditor.Data;

namespace Haztech.SpriteEditor.Editor
{
    public class ToolWindow : EditorWindow
    {
        [SerializeField] private SpriteConfig config;
        const string LastConfigKey = "ScriptEditor.LastConfig";

        public SpriteConfig SpriteConfig => config;
        [SerializeField] public int selectedLayer = -1;
        [SerializeField] public int selectedState = -1;

        [OnOpenAsset]
        public static bool OnOpenAsset(EntityId entityId, int line)
        {
            string path = AssetDatabase.GetAssetPath(entityId);

            SpriteConfig openedConfig =
                AssetDatabase.LoadAssetAtPath<SpriteConfig>(path);

            if (openedConfig == null)
                return false;

            ToolWindow window = GetWindow<ToolWindow>("Sprite Editor");

            window.OpenConfig(path);
            window.Show();
            window.Focus();

            return true;
        }

        private void OnEnable()
        {
            string path = EditorPrefs.GetString(LastConfigKey, "");
            if (!string.IsNullOrEmpty(path))
            {
                config = AssetDatabase.LoadAssetAtPath<SpriteConfig>(path);
            }
        }

        [MenuItem("Tools/Sprite Editor")]
        public static void ShowWindow()
        {
            GetWindow<ToolWindow>("Sprite Editor");
        }

        void OnGUI()
        {
            Toolbar.Draw(this);

            if (config == null) return;

            EditorGUILayout.BeginHorizontal();

            LayerPanel.Draw(this);
            DisplayPanel.Draw(this);
            PropertiesPanel.Draw(this);
            EditorGUILayout.EndHorizontal();
        }

        public void NewConfig(string path)
        {
            SpriteConfig newConfig = ScriptableObject.CreateInstance<SpriteConfig>();

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
            config = AssetDatabase.LoadAssetAtPath<SpriteConfig>(path);
        }
    }
}