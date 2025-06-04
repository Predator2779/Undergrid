#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Grid;

public static class HexDefinitionGenerator
{
    [MenuItem("Tools/Hex Dig/Create Hex Definitions")]
    public static void Generate()
    {
        string folder = "Assets/Configs/Hexes/";
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        var hexes = new List<HexDataTemplate>
        {
            new("dirt", HexType.Soil, ColorFromHex("#8B4513"), GetSprite("Hex"), 0, 1, 0.6f, 1.0f),
            new("stone", HexType.Stone, ColorFromHex("#808080"), GetSprite("Hex"), 0, 2, 1.0f, 0.5f),
            new("clay", HexType.Soil, ColorFromHex("#B5651D"), GetSprite("Hex"), 1, 2, 1.3f, 0.3f),
            new("gravel", HexType.Stone, ColorFromHex("#7D7D7D"), GetSprite("Hex"), 1, 2, 1.1f, 0.2f),
            new("coal", HexType.Ore, ColorFromHex("#2F2F2F"), GetSprite("Hex"), 5, 3, 1.5f, 0.15f),
            new("copper", HexType.Ore, ColorFromHex("#B87333"), GetSprite("Hex"), 6, 3, 1.6f, 0.12f),
            new("iron", HexType.Ore, ColorFromHex("#A19D94"), GetSprite("Hex"), 8, 4, 1.8f, 0.10f),
            new("silver", HexType.Ore, ColorFromHex("#C0C0C0"), GetSprite("Hex"), 12, 5, 2.0f, 0.06f),
            new("gold", HexType.Ore, ColorFromHex("#FFD700"), GetSprite("Hex"), 20, 6, 2.5f, 0.03f),
            new("platinum", HexType.Ore, ColorFromHex("#E5E4E2"), GetSprite("Hex"), 30, 7, 3.0f, 0.01f),
            new("lava", HexType.Trap, ColorFromHex("#FF4500"), GetSprite("Hex"), 0, 0, 0.0f, 0.01f),
            new("void", HexType.Void, ColorFromHex("#000000"), GetSprite("Hex"), 0, 0, 0.0f, 0.02f),
            new("crystal", HexType.Artifact, ColorFromHex("#00FFFF"), GetSprite("Hex"), 50, 10, 3.5f, 0.005f),
            new("energy", HexType.PowerUp, ColorFromHex("#00FF00"), GetSprite("Hex"), 0, -10, 1.0f, 0.03f)
        };

        var settings = AddressableAssetSettingsDefaultObject.Settings;
        
        foreach (var h in hexes)
        {
            string path = folder + h.Id + ".asset";
            var asset = ScriptableObject.CreateInstance<HexDefinition>();

            asset.Id = h.Id;
            asset.Type = h.Type;
            asset.Color = h.Color;
            asset.Sprite = h.Sprite;
            asset.Value = h.Value;
            asset.EnergyCost = h.EnergyCost;
            asset.DigTime = h.DigTime;
            asset.Weight = h.Weight;

            AssetDatabase.CreateAsset(asset, path);

            string guid = AssetDatabase.AssetPathToGUID(path);
            var entry = settings.CreateOrMoveEntry(guid, settings.DefaultGroup);
            entry.SetLabel("Hexes", true);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("HexDefinitions generated.");
    }

    private static Sprite GetSprite(string id)
    {
        string spritePath = $"Assets/Sprites/Hexes/{id}.png";
        return AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
    }
    
    private static Color ColorFromHex(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out var color))
            return color;
        return Color.white;
    }

    private class HexDataTemplate
    {
        public string Id { get; }
        public HexType Type { get; }
        public Color Color { get; }
        public Sprite Sprite { get; }
        public int Value { get; }
        public int EnergyCost { get; }
        public float DigTime { get; }
        public float Weight { get; }

        public HexDataTemplate(
            string id, HexType type, Color color, Sprite sprite,
            int value, int energyCost, float digTime, float weight)
        {
            Id = id;
            Type = type;
            Color = color;
            Sprite = sprite;
            Value = value;
            EnergyCost = energyCost;
            DigTime = digTime;
            Weight = weight;
            
        }
    }
}
#endif