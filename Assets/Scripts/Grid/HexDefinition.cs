using UnityEngine;

namespace Grid
{
    [CreateAssetMenu(fileName = "HexDefinition", menuName = "Hex Dig/Hex Definition", order = 0)]
    public class HexDefinition : ScriptableObject
    {
        public string Id;
        public HexType Type;
        public Sprite Sprite;
        public Color Color = Color.white;

        [Range(0, 100)] public int Value;
        [Range(1, 30)] public int EnergyCost;
        [Range(0, 10)] public float DigTime;
        [Range(0, 1)] public float Weight;
    }
}