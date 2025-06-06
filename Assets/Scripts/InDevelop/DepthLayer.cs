using System.Collections.Generic;

namespace InDevelop
{
    [System.Serializable]
    public class DepthLayer
    {
        public int MinDepth;
        public int MaxDepth;
        public List<HexSpawnEntry> Entries;
        public bool IsTechnicalLayer;
    }
}