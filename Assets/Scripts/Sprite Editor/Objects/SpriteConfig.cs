using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Haztech.SpriteEditor.Data
{
    public class SpriteConfig : ScriptableObject
    {
        private List<Layer> layers = new List<Layer>();
        private List<StateConfig> states = new List<StateConfig>();
        public int LayerCount => layers.Count;
        public IEnumerable<Layer> Layers => layers;
        public int StateCount => states.Count;
        public IEnumerable<StateConfig> States => states;

        public void AddLayer(Layer layer)
        {
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
        }

        public StateConfig GetState(int stateIndex)
        {
            if (stateIndex >= states.Count)
            {
                Debug.LogWarning(": Invalid state index in editor config");
                return null;
            }

            return states[stateIndex];
        }

        public void RemoveState(int stateIndex)
        {
            if (stateIndex >= states.Count) return;

            states.RemoveAt(stateIndex);
        }

        public void MoveStateDown(int index)
        {
            MoveState(index, 1);
        }

        public void MoveStateUp(int index)
        {
            MoveState(index, -1);
        }

        private void MoveState(int index, int dir)
        {
            (states[index], states[index + dir]) = (states[index + dir], states[index]);
        }
    }
}
