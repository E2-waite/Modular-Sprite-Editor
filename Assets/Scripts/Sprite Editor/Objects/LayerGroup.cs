using System.Collections.Generic;
using UnityEngine;

namespace Haztech.SpriteEditor.Data
{
    [System.Serializable]
    public class LayerGroup : LayerObject
    {
        public int id = 0;
        private SpriteConfig config;
        public bool expanded = false;
        [SerializeReference] private List<Layer> layers = new List<Layer>();
        public List<Layer> Layers => layers;
        public bool IsEmpty => layers.Count == 0;
        public LayerGroup(string name, SpriteConfig config)
        {
            this.name = name;
            this.config = config;
        }

        public void AddLayer(Layer layer)
        {
            if (!layers.Contains(layer))
            {
                layers.Add(layer);
                layer.SetGroup(this);
            }
        }

        public void RemoveLayer(Layer layer)
        {
            if (layer != null && layers != null)
            {
                layers.Remove(layer);
            }
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

        public void Expand(bool expand)
        {
            if (expanded != expand)
            {
                expanded = expand;
                config?.RefeshExpandedList();
            }
        }

        public void ToggleExpand()
        {
            expanded = !expanded;
            config?.RefeshExpandedList();
        }
    }
}