using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private Vector3 _axisSpeed;
    [SerializeField] private Space _space;

    private void Update()
    {
        transform.Rotate(_axisSpeed * Time.deltaTime, _space);
    }
}
