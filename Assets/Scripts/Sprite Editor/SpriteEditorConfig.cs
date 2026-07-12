using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

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

}
