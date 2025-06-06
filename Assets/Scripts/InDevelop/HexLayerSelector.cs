using System.Linq;
using UnityEngine;
using Grid;

namespace InDevelop
{
    public static class HexLayerSelector
    {
        public static HexDefinition Choose(int depth, LayeredHexTable table, int blendChance = 0)
        {
            var currentLayer = table.Layers.FirstOrDefault(layer => layer.MinDepth <= depth && depth <= layer.MaxDepth);
            
            if (currentLayer == null)
            {
                currentLayer = table.Layers
                    .Where(layer => !layer.IsTechnicalLayer)
                    .OrderByDescending(layer => layer.MaxDepth)
                    .FirstOrDefault();

                if (currentLayer == null || currentLayer.Entries.Count == 0)
                    throw new System.Exception($"No layers defined or non-empty at depth = {depth}");
            }

            bool canBlend = !currentLayer.IsTechnicalLayer;
            
            DepthLayer lowerLayer = null;
            if (canBlend)
            {
                lowerLayer = table.Layers
                    .Where(layer => layer.MinDepth > currentLayer.MaxDepth && !layer.IsTechnicalLayer)
                    .OrderByDescending(layer => layer.MinDepth)
                    .LastOrDefault();
            }

            // Расчет адаптивного шанса
            bool useLower = false;
            if (canBlend && lowerLayer != null && lowerLayer.Entries.Count > 0 && blendChance > 0)
            {
                int layerHeight = currentLayer.MaxDepth - currentLayer.MinDepth;
                if (layerHeight > 0)
                {
                    float t = Mathf.InverseLerp(currentLayer.MinDepth, currentLayer.MaxDepth, depth); // 0 (верх) → 1 (низ)
                    float adjustedChance = blendChance * t; // ближе к низу — больше шанс
                    int roll = Random.Range(0, 100);
                    useLower = roll < adjustedChance;
                }
            }

            var chosenLayer = useLower ? lowerLayer : currentLayer;

            float total = chosenLayer.Entries.Sum(e => e.Weight);
            float rollFinal = Random.Range(0, total);
            float accum = 0f;

            foreach (var entry in chosenLayer.Entries)
            {
                accum += entry.Weight;
                if (rollFinal <= accum)
                {
                    return entry.Hex;
                }
            }

            return chosenLayer.Entries[0].Hex;
        }
    }
}
