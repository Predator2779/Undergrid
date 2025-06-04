using System;
using Grid;
using UnityEngine;

[Serializable]
public class HexData
{
    [SerializeField] private HexDefinition _definition;
    public HexDefinition Definition => _definition;

    public HexData(HexDefinition definition)
    {
        _definition = definition;
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