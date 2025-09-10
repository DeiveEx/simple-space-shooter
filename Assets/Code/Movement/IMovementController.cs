using UnityEngine;

public interface IMovementController
{
    float Speed { get; }
    
    void SetDirection(Vector2 direction);
}
