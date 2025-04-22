using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{
    public UnityEvent OnTrigger;
    public float gravityScale = 2f;
    private float velocityY = 0f;

    private void Update()
    {
        float gravity = 9.8f * gravityScale;
        velocityY += gravity * Time.deltaTime;

        transform.Translate(velocityY * Time.deltaTime * Vector3.down);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTrigger?.Invoke();
    }

    public void SetVelocityY(float newVelocityY)
    {
        velocityY = newVelocityY;
    }
}
