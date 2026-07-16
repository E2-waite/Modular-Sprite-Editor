using UnityEngine;

namespace Haztech.SpriteEditor.Data
{
    [System.Serializable]
    public class SpriteData
    {
        public Sprite sprite = null;

        public SpriteData()
        {

        }

        public SpriteData(SpriteData other)
        {
            sprite = other.sprite;
        }
    }
}
