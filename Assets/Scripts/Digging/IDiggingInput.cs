using UnityEngine;

public interface IDiggingInput
{
    public bool IsDigging { get; }
    public bool IsCanceling { get; }
    
    public Vector2 GetDirection();
}