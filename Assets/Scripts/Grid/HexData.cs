using UnityEngine;

public class HexData
{
    public Sprite Sprite { get; set; }
    public Color Color { get; set; }

    public HexData(Sprite sprite, Color color)
    {
        Sprite = sprite;
        Color = color;
    }
}