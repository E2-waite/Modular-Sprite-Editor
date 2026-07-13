using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

namespace SpriteEditor.Data
{
    public class SpriteEditorConfig : ScriptableObject
    {
        [SerializeField] private List<SpriteLayer> layers = new List<SpriteLayer>();
        public int LayerCount => layers.Count;
        public IEnumerable<SpriteLayer> Layers => layers;

        public void AddLayer(SpriteLayer layer)
        {
            layers.Add(layer);
        }

        public SpriteLayer GetLayer(int layerIndex)
        {
            if (layerIndex >= layers.Count)
            {
                Debug.LogWarning("SpriteEditor: Invalid layer index in editor config");
                return null;
            }

            return layers[layerIndex];
        }

        public void RemoveLayer(int layerIndex)
        {
            if (layerIndex >= layers.Count) return;

            layers.RemoveAt(layerIndex);
        }

        public void MoveDown(int index)
        {
            MoveLayer(index, 1);
        }

        public void MoveUp(int index)
        {

            MoveLayer(index, -1);

        }

        private void MoveLayer(int index, int dir)
        {
            (layers[index], layers[index + dir]) = (layers[index + dir], layers[index]);
        }
    }
}
