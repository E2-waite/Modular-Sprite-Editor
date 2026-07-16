using UnityEngine;
namespace Haztech.SpriteEditor.Data
{
    [System.Serializable]
    public class StateData
    {
        public SpriteData[] directions = new SpriteData[(int)Direction.Max];

        public StateData()
        {
            for (int i = 0; i < (int)Direction.Max; i++)
            {
                if (directions[i] == null) directions[i] = new SpriteData();
            }
        }

        public SpriteData GetData(Direction dir)
        {
            return directions[(int)dir];
        }
    }
}
