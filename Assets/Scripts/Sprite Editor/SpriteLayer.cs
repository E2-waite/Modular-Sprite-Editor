using UnityEngine;

namespace SpriteEditor.Data
{
    [System.Serializable]
    public class SpriteLayer : ScriptableObject
    {
        public Sprite sprite = null;
        public Color color = Color.white;
        public bool visible = true;

        public SpriteLayer(string name)
        {
            this.name = name;
        }
    }
}