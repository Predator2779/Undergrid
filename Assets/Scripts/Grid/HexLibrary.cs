using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grid;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.AddressableAssets;

public static class HexLibrary
{
    private static Dictionary<string, HexData> _definitions;

    public static async Task InitializeAsync()
    {
        _definitions = new Dictionary<string, HexData>();

#if UNITY_EDITOR
        // В редакторе — через AssetDatabase
        var guids = AssetDatabase.FindAssets("t:HexDefinition");
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var def = AssetDatabase.LoadAssetAtPath<HexDefinition>(path);
            if (def != null)
                _definitions[def.Id] = new HexData(def);
        }
#else
        // В билде — через Addressables
        var hexDefs = await Addressables.LoadAssetsAsync<HexDefinition>("Hexes", null).Task;
        foreach (var def in hexDefs)
        {
            _definitions[def.Id] = new HexData(def);
        }
#endif
    }

    public static HexData Get(string id)
    {
        if (_definitions.TryGetValue(id, out var def))
            return def;

        throw new System.Exception($"HexDefinition not found: {id}");
    }

    // public static IEnumerable<HexData> All => _definitions.Values;
    //
    // public static HexData ChooseWeighted(List<HexData> defs)
    // {
    //     float totalWeight = defs.Sum(d => d.Weight);
    //     float roll = Random.Range(0, totalWeight);
    //     float accum = 0;
    //
    //     foreach (var def in defs)
    //     {
    //         accum += def.Weight;
    //         if (roll <= accum)
    //             return def;
    //     }
    //
    //     return defs[0];
    // }
}