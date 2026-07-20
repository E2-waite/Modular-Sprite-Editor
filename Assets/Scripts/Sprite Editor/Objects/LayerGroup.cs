using System.Collections.Generic;
using UnityEngine;

namespace Haztech.SpriteEditor.Data
{
    [System.Serializable]
    public class LayerGroup : LayerObject
    {
        [SerializeField] private List<Layer> layers = new List<Layer>();
        public List<Layer> Layers => layers;
        public LayerGroup(string name)
        {
            this.name = name;
        }
        public void AddLayer(Layer layer)
        {
            if (!layers.Contains(layer))
                layers.Add(layer);
        }

        public void RemoveLayer(Layer layer)
        {
            layers.Remove(layer);
        }

        public void RemoveLayer(int index)
        {
            if (index < layers.Count)
                layers.RemoveAt(index);
        }

        public Layer GetLayer(int index)
        {
            if (index < layers.Count) return layers[index];
            return null;
        }
    }
}