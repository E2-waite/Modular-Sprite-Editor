using UnityEngine;

[System.Serializable]
public class SpriteLayer : ScriptableObject
{
    public string name = "New Layer";
    public Sprite sprite = null;
    public Color color = Color.white;
    public bool visible = true;
}
