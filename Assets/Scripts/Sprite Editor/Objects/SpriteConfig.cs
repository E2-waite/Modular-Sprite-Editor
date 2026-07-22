using System.Collections.Generic;
using UnityEngine;

namespace Haztech.SpriteEditor.Data
{
    public class SpriteConfig : ScriptableObject
    {
        [SerializeReference] private List<LayerObject> layerObjects = new List<LayerObject>();
        [SerializeField] private List<StateConfig> states = new List<StateConfig>();
        [SerializeField] private List<ColorGroup> colorGroups = new List<ColorGroup>();

        private List<LayerObject> expanded = new List<LayerObject>();



        public int selectedLayer = 0;
        public int selectedState = 0;
        public Direction selectedDir = Direction.South;

        public int LayerCount => layerObjects.Count;
        //public IEnumerable<Layer> Layers => layers;
        public List<LayerObject> ExpandedLayers => expanded;
        public int StateCount => states.Count;
        public List<StateConfig> States => states;
        public List<ColorGroup> ColorGroups => colorGroups;

        private void OnEnable()
        {
            RefreshGroupRef();
            RefeshExpandedList();
        }

        public List<Layer> GetLayers()
        {
            List<Layer> layers = new List<Layer>();
            foreach (LayerObject layerObj in layerObjects)
            {
                if (layerObj == null) continue;

                if (layerObj is Layer layer)
                {
                    layers.Add(layer);
                }
                else if (layerObj is LayerGroup group)
                {
                    foreach (Layer groupedLayer in group.Layers)
                    {
                        layers.Add(groupedLayer);
                    }
                }
            }
            return layers;
        }

        public Sprite GetSprite(int layerId, Direction dir = Direction.Null)
        {
            if (layerId >= 0 && layerId < layerObjects.Count)
            {
                Layer layer = (Layer)layerObjects[layerId];

                if (layer == null) return null;

                StateData state = layer.GetState(selectedState);

                if (state == null) return null;

                SpriteData spriteData = state.GetData(dir == Direction.Null ? selectedDir : dir);

                return spriteData.sprite;
            }

            return null;
        }

        public Color GetColor(int layerId)
        {
            if (layerId >= 0 && layerId < layerObjects.Count)
            {
                Layer layer = (Layer)layerObjects[layerId];

                if (layer == null) return Color.white;

                if (layer.colorGroupId >= 0 && layer.colorGroupId < colorGroups.Count)
                    return colorGroups[layer.colorGroupId].color;
                else
                    return layer.color;
            }
            return Color.white;

        }


        public void AddLayer(Layer layer)
        {
            for (int i = 0; i < states.Count; i++)
            {
                layer.AddState(new StateData());
            }

            layerObjects.Add(layer);

            RefeshExpandedList();
        }

        public void AddGroup(LayerGroup group)
        {
            layerObjects.Add(group);
            RefeshExpandedList();
        }

        public Layer GetLayer(int layerIndex)
        {
            if (layerIndex >= expanded.Count)
            {
                Debug.LogWarning(": Invalid layer index in editor config");
                return null;
            }

            if (expanded[layerIndex] is Layer layer) return layer;
            return null;
        }

        public LayerObject GetLayerObj(int index)
        {
            if (index >= layerObjects.Count)
            {
                Debug.LogWarning(": Invalid layer index in editor config");
                return null;
            }

            return layerObjects[index];
        }

        public void RemoveLayer(int layerIndex)
        {
            if (layerIndex >= layerObjects.Count) return;

            layerObjects.RemoveAt(layerIndex);

            RefeshExpandedList();
        }

        public void RemoveLayer(LayerObject layerObj)
        {
            layerObjects.Remove(layerObj);

            RefeshExpandedList();
        }

        public void MoveLayerDown(int index)
        {
            MoveLayer(index, index + 1);

            RefeshExpandedList();
        }

        public void MoveLayerUp(int index)
        {
            MoveLayer(index, index - 1);

            RefeshExpandedList();
        }

        public void MoveLayer(int index, int toIndex)
        {
            LayerObject layerObj = layerObjects[index];
            layerObjects.RemoveAt(index);
            layerObjects.Insert(toIndex, layerObj);

            RefeshExpandedList();
        }

        public void AddState(StateConfig state)
        {
            states.Add(state);
            foreach (Layer layer in layerObjects)
            {
                if (selectedState >= 0 && selectedState < layer.states.Count)
                    layer.AddState(new StateData(layer.states[selectedState]));
                else
                    layer.AddState(new StateData());
            }
        }

        public StateConfig GetStateConfig(int stateIndex)
        {
            if (stateIndex >= states.Count)
            {
                Debug.LogWarning("SpriteEditor: Invalid state index in editor config");
                return null;
            }

            return states[stateIndex];
        }

        public void RemoveState(int stateIndex)
        {
            if (stateIndex >= states.Count) return;

            states.RemoveAt(stateIndex);
            foreach (Layer layer in layerObjects)
            {
                layer.RemoveState(stateIndex);
            }
        }

        public SpriteData GetData(int layerId, int stateId, Direction dir)
        {
            Layer layer = (Layer)layerObjects[layerId];

            if (layer == null) return null;

            StateData state = layer.GetState(stateId);

            if (state == null) return null;

            return state.GetData(dir);
        }

        public void AddLayerToGroup(Layer layer, LayerGroup group)
        {
            if (layer == null || group == null) return;

            // Clear existing storage
            if (layer.InGroup)
            {
                layer.Group.RemoveLayer(layer);
            }
            else
            {
                layerObjects.Remove(layer);
            }

            group.AddLayer(layer);

            RefeshExpandedList();
        }

        public void RefeshExpandedList()
        {
            expanded.Clear();

            foreach (LayerObject layerObj in layerObjects)
            {
                if (layerObj is Layer layer)
                {
                    expanded.Add(layer);
                }
                else if (layerObj is LayerGroup group)
                {
                    expanded.Add(group);
                    
                    if (group.expanded)
                    {
                        foreach (Layer groupLayer in group.Layers)
                        {
                            expanded.Add(groupLayer);
                        }
                    }
                }
            }
        }

        private void RefreshGroupRef()
        {
            foreach (LayerObject layerObj in layerObjects)
            {
                if (layerObj is LayerGroup group)
                {
                    foreach (Layer layer in group.Layers)
                    {
                        layer.SetGroup(group);
                    }
                }
                else if (layerObj is Layer layer)
                {
                    layer.ClearGroup();
                }
            }
        }
    }
}
