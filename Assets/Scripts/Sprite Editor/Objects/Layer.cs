using System.Collections.Generic;
using UnityEngine;

namespace Haztech.SpriteEditor.Data
{
    [System.Serializable]
    public class Layer : ScriptableObject
    {
        public Sprite sprite = null;
        public Color color = Color.white;
        public bool visible = true;

        public Layer(string name)
        {
            this.name = name;
        }
    }
}