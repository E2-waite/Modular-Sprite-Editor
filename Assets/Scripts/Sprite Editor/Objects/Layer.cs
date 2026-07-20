using System.Collections.Generic;
using UnityEngine;

namespace Haztech.SpriteEditor.Data
{
    [System.Serializable]
    public class Layer
    {
        public string name;
        public Color color = Color.white;
        public int colorGroupId = -1;
        public bool visible = true;
        public List<StateData> states = new List<StateData>();

        public Layer(string name)
        {
            this.name = name;
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