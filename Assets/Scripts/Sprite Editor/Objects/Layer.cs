using System.Collections.Generic;
using UnityEngine;

namespace Haztech.SpriteEditor.Data
{
    [System.Serializable]
    public class Layer : LayerObject
    {
        private SpriteConfig config;
        public Color color = Color.white;
        public int colorGroupId = -1;
        public List<StateData> states = new List<StateData>();
        private LayerGroup group = null;
        public bool InGroup => group != null;
        public LayerGroup Group => group;
        public Layer(string name, SpriteConfig config)
        {
            this.name = name;
            this.config = config;
        }

        public void SetGroup(LayerGroup newGroup)
        {
            group = newGroup;
        }

        public void ClearGroup()
        {
            group = null;
        }

        public StateData GetState(int index)
        {
            if (index < 0 || index >= states.Count ) return null;

            return states[index];
        }

        public void AddState(StateData data)
        {
            states.Add(data);
        }

        public void RemoveState(int index)
        {
            states.RemoveAt(index);
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