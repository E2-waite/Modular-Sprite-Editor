using Haztech.SpriteEditor.Data;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Haztech.SpriteEditor.Editor
{
    public class Window : EditorWindow
    {
        [SerializeField] private SpriteConfig config;
        const string LastConfigKey = "ScriptEditor.LastConfig";

        public SpriteConfig SpriteConfig => config;

        public static Window Instance;

        [OnOpenAsset]
        public static bool OnOpenAsset(EntityId entityId, int line)
        {
            string path = AssetDatabase.GetAssetPath(entityId);

            SpriteConfig openedConfig =
                AssetDatabase.LoadAssetAtPath<SpriteConfig>(path);

            if (openedConfig == null)
                return false;

            Instance = GetWindow<Window>("Sprite Editor");

            Instance.OpenConfig(path);
            Instance.Show();
            Instance.Focus();

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
            Instance = GetWindow<Window>("Sprite Editor");
        }

        void OnGUI()
        {
            Toolbar.Draw();

            if (config == null) return;

            EditorGUILayout.BeginHorizontal();

            SelectPanel.Draw();
            DisplayPanel.Draw();
            PropertiesPanel.Draw();
            EditorGUILayout.EndHorizontal();

            if (config != null)
                EditorUtility.SetDirty(config);
        }

        public void NewConfig(string path)
        {
            SpriteConfig newConfig = ScriptableObject.CreateInstance<SpriteConfig>();

            newConfig.AddLayer(new Layer("New Layer", newConfig));
            newConfig.AddState(new StateConfig("New State"));
            config.selectedLayer = 0;
            config.selectedState = 0;

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