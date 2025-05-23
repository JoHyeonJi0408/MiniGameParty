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
        var lastVelocity = collision.relativeVelocity;
        Vector2 direction = lastVelocity.normalized;

        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Brick"))
        {
            var normal = collision.contacts[0].normal;

            if (Vector2.Dot(normal, Vector2.down) > 0.9f || (Vector2.Dot(normal, Vector2.up) > 0.9f))
            {
                direction = Vector2.Reflect(lastVelocity.normalized, Vector2.right.normalized);
            }
            else if (Vector2.Dot(normal, Vector2.right) > 0.9f || Vector2.Dot(normal, Vector2.left) > 0.9f)
            {
                direction = Vector2.Reflect(lastVelocity.normalized, Vector2.up.normalized);
            }

            if (collision.gameObject.CompareTag("Brick"))
            {
                OnPointScored.Invoke();
                Destroy(collision.gameObject);
            }
        }
        else if(collision.gameObject.CompareTag("Paddle"))
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
