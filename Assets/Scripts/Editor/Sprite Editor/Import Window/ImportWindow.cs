using Haztech.SpriteEditor.Data;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Haztech.SpriteEditor.Editor
{
    public class ImportWindow : EditorWindow
    {
        private static int selectedLayer = 0, selectedState = 0;
        public static void Open()
        {
            GetWindow<ImportWindow>("Import Sprite Sheet");
        }

        void OnGUI()
        {
            GUILayout.Label("", EditorStyles.boldLabel);

            SpriteConfig config = Window.Instance.SpriteConfig;

            DrawLayerDropdown(config);
            DrawStateDropdown(config);
        }

        private void DrawLayerDropdown(SpriteConfig config)
        {
            List<Layer> layers = config.GetLayers();

            string[] options = new string[layers.Count];
            for (int i = 0; i < layers.Count; i++)
            {
                Layer layer = layers[i];
                options[i] = layer.name;
            }

            selectedLayer = EditorGUILayout.Popup(selectedLayer, options);
        }

        private void DrawStateDropdown(SpriteConfig config)
        {
            List<StateConfig> states = config.States;

            string[] options = new string[states.Count];
            for (int i = 0; i < states.Count; i++)
            {
                StateConfig state = states[i];
                options[i] = state.name;
            }

            selectedState = EditorGUILayout.Popup(selectedState, options);
        }
    }
}