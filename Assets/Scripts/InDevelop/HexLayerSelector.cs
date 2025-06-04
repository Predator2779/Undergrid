using System.Linq;
using Grid;
using UnityEngine;

namespace InDevelop
{
    public static class HexLayerSelector
    {
        public static HexDefinition Choose(int y, LayeredHexTable table)
        {
            var layer = table.Layers.FirstOrDefault(l => y >= l.MinY && y <= l.MaxY);
            
            if (layer == null)
            {
                layer = table.Layers.OrderByDescending(l => l.MaxY).FirstOrDefault();
                if (layer == null || layer.Entries.Count == 0)
                {
                    throw new System.Exception("No layers defined in the table.");
                }
            }
            
            float total = layer.Entries.Sum(e => e.Weight);
            float roll = Random.Range(0, total);
            float accum = 0;

            foreach (var entry in layer.Entries)
            {
                accum += entry.Weight;
                if (roll <= accum)
                {
                    return entry.Hex;
                }
            }

            return layer.Entries[0].Hex;
        }
    }
}