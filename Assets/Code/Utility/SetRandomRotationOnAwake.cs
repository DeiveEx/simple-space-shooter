using UnityEngine;

public class SetRandomRotationOnAwake : MonoBehaviour
{
    private void Awake()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
    }
}
