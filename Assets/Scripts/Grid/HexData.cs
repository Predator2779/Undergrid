using UnityEngine;

public class HexData
{
    public HexDefinition Definition { get; }

    public HexData(HexDefinition definition)
    {
        Definition = definition;
    }

    public string Id => Definition.Id;
    public HexType Type => Definition.Type;
    public Sprite Sprite => Definition.Sprite;
    public Color Color => Definition.Color;
    public int Value => Definition.Value;
    public int EnergyCost => Definition.EnergyCost;
    public float DigTime => Definition.DigTime;
    public float Weight => Definition.Weight;
}