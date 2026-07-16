using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Haztech.SpriteEditor.Data
{
    public class SpriteConfig : ScriptableObject
    {
        private List<Layer> layers = new List<Layer>();
        private List<StateConfig> states = new List<StateConfig>();

        public int selectedLayer = 0;
        public int selectedState = 0;
        public Direction selectedDir = Direction.South;

        public int LayerCount => layers.Count;
        public IEnumerable<Layer> Layers => layers;
        public int StateCount => states.Count;
        public IEnumerable<StateConfig> States => states;

        public void AddLayer(Layer layer)
        {
            for (int i = 0; i < states.Count; i++)
            {
                layer.AddState(new StateData());
            }

            layers.Add(layer);
        }

        public Layer GetLayer(int layerIndex)
        {
            if (layerIndex >= layers.Count)
            {
                Debug.LogWarning(": Invalid layer index in editor config");
                return null;
            }

            return layers[layerIndex];
        }

        public void RemoveLayer(int layerIndex)
        {
            if (layerIndex >= layers.Count) return;

            layers.RemoveAt(layerIndex);
        }

        public void MoveLayerDown(int index)
        {
            MoveLayer(index, 1);
        }

        public void MoveLayerUp(int index)
        {
            MoveLayer(index, -1);
        }

        private void MoveLayer(int index, int dir)
        {
            (layers[index], layers[index + dir]) = (layers[index + dir], layers[index]);
        }

        public void AddState(StateConfig state)
        {
            states.Add(state);
            foreach (Layer layer in layers)
            {
                layer.AddState(new StateData(layer.states[selectedState]));
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
            foreach (Layer layer in layers)
            {
                layer.RemoveState(stateIndex);
            }
        }

        public SpriteData GetData(int layerId, int stateId, Direction dir)
        {
            Layer layer = layers[layerId];

            if (layer == null) return null;

            StateData state = layer.GetState(stateId);

            if (state == null) return null;

            return state.GetData(dir);
        }
    }
}
