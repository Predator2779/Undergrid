using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Hex : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private HexData _data;
    
    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _data = new HexData(_renderer.sprite, _renderer.color);
    }

    public void SetColor(Color color)
    {
        _renderer.color = color;
    }
    
    public void SetHex(HexData data)
    {
        _renderer.sprite = data.Sprite;
        _renderer.color = data.Color;
    }

    public void ResetHex()
    {
        _renderer.sprite = _data.Sprite;
        _renderer.color = _data.Color;
    }
}

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