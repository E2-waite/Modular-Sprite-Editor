using UnityEngine;
namespace Haztech.SpriteEditor.Data
{
    [System.Serializable]
    public class State
    {
        Direction[] directions = new Direction[(int)Directions8D.Max];
    }
}
