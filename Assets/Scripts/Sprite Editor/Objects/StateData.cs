using UnityEngine;
namespace Haztech.SpriteEditor.Data
{
    [System.Serializable]
    public class StateData
    {
        public SpriteData[] directions = new SpriteData[(int)Direction.Null];


        public StateData()
        {
            for (int i = 0; i < (int)Direction.Null; i++)
            {
                if (directions[i] == null) directions[i] = new SpriteData();
            }
        }

        public StateData(StateData other)
        {
            for (int i = 0; i < (int)Direction.Null; i++)
            {
                if (directions[i] == null) directions[i] = new SpriteData(other.directions[i]);
            }
        }

        public SpriteData GetData(Direction dir)
        {
            return directions[(int)dir];
        }
    }
}
