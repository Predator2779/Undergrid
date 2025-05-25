using UnityEngine;

public class DiggingInputPC : IDiggingInput
{
    private readonly Transform _origin;
    
    public bool IsDigging => Input.GetMouseButton(0);
    public bool IsCanceling => !Input.GetMouseButtonDown(0);

    public DiggingInputPC(Transform origin)
    {
        _origin = origin;
    }

    public Vector2 GetDirection()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        return mouseWorld - _origin.position;
    }
}