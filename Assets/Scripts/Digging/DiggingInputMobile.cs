// using UnityEngine;
//
// public class DiggingInputMobile : IDiggingInput
// {
//     public bool IsDigging => Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary;
//     public bool IsCanceling => false; // добавить кнопку в UI
//
//     public Vector2 GetDirection()
//     {
//         // Placeholder — можно сделать свайпы или UI кнопки
//         return Vector2.down; 
//     }
// }

// public class DiggingInputMobile : IDiggingInput
// {
//     public bool IsDigging => MobileInput.Instance.IsDigButtonHeld; // Needs real implementation
//     public bool IsCanceling => !IsDigging;
//
//     public Vector2 GetDirection()
//     {
//         return MobileInput.Instance.GetJoystickDirection(); // Needs real implementation
//     }
// }