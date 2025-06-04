using System.Collections.Generic;

namespace InDevelop
{
    [System.Serializable]
    public class DepthLayer
    {
        public int MinY; // например: 0
        public int MaxY; // например: 5
        public List<HexSpawnEntry> Entries;
    }
}