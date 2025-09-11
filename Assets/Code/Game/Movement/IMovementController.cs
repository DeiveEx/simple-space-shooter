using UnityEngine;

public interface IMovementController
{
    float Speed { get; }

    void Setup(float speed);
    void SetDirection(Vector2 direction);
}
