using System.Collections.Generic;
using UnityEngine;

namespace InDevelop
{
    [CreateAssetMenu(menuName = "Hex Dig/LayeredHexTable")]
    public class LayeredHexTable : ScriptableObject
    {
        public List<DepthLayer> Layers;
    }
}