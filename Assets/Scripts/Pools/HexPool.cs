namespace Common
{
    public class HexPool : ObjectPool<Hex>
    {
        [EditorButton("Create Objects")]
        private void CreateObjects() => CreateObjects(_amount);

        [EditorButton("Clear")]
        private void Clear()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
            Pool.Clear();
        }
    }
}