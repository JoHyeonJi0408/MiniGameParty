using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnTrigger;
    public Animator animator;
    private float gravityScale = 2f;
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

    public void PlayAnimation(string trigger)
    {
        animator.SetTrigger(trigger);
    }

    public void Reset()
    {
        PlayAnimation("Fly");
        transform.position = new Vector3(-15, 0, 0);
    }
}
