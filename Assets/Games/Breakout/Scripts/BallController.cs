using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallController : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;

    [HideInInspector] public UnityEvent OnPointScored;

    private void Start()
    {
        rb.velocity = new Vector2(0.5f, 1).normalized * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 direction = rb.velocity.normalized;
        var lastVelocity = collision.relativeVelocity;

        if (collision.gameObject.CompareTag("Wall"))
        {
            direction = Vector2.Reflect(lastVelocity.normalized, rb.velocity.normalized);
        }
        else if (collision.gameObject.CompareTag("Brick"))
        {
            OnPointScored.Invoke();
            Destroy(collision.gameObject);
            direction = Vector2.Reflect(lastVelocity.normalized, rb.velocity.normalized);
        }
        else if (collision.gameObject.CompareTag("Paddle"))
        {
            float paddleX = collision.transform.position.x;
            float contactX = collision.contacts[0].point.x;
            float relativeX = (contactX - paddleX) / (collision.collider.bounds.size.x / 2f);

            float bounceAngle = Mathf.Lerp(1f, 0.7f, Mathf.Abs(relativeX));
            direction = new Vector2(relativeX, bounceAngle).normalized;
        }

        rb.velocity = direction * speed;
    }
}
