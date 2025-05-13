using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;

    private void Start()
    {
        rb.velocity = Vector2.up.normalized * speed;
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
            collision.gameObject.SetActive(false);

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
