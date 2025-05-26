using UnityEngine.Events;

namespace DefaultNamespace
{
    public static class EventBus
    {
        public static UnityEvent<Hex> OnHexRemoved = new();
    }
}