using DefaultNamespace;

namespace Common
{
    public class HexPool : ObjectPool<Hex>
    {
        private void OnEnable()
        {
            EventBus.OnHexRemoved.AddListener(Return);
        }
        
        private void OnDisable()
        {
            EventBus.OnHexRemoved.RemoveListener(Return);
        }

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