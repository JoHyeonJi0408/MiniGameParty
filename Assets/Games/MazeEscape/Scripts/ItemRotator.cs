using UnityEngine;

public class ItemRotator : MonoBehaviour
{
    public Vector3 rotationSpeed;

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
