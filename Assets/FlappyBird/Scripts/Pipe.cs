using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pipe : MonoBehaviour
{
    public UnityEvent OnBecameInvisible;
    public SpriteRenderer topRenderer;
    public SpriteRenderer bottomRenderer;
    public BoxCollider2D topCollider;
    public BoxCollider2D bottomCollider;
    private float moveSpeed = 10f;

    private void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.left);

        if (transform.position.x < -40f)
        {
            OnBecameInvisible?.Invoke();
        }
    }

    public void ResetPipe()
    {
        int topY = Random.Range(1, 30);
        int bottomY = 30 - topY;

        if (bottomY < 1)
        {
            bottomY = 1;
            topY = 29;
        }

        topRenderer.size = new Vector2(7f, topY);
        bottomRenderer.size = new Vector2(7f, bottomY);
        topCollider.size = new Vector2(7, topY);
        bottomCollider.size = new Vector2(7, bottomY);
        topCollider.offset = new Vector2(0, -topY / 2);
        bottomCollider.offset = new Vector2(0, bottomY / 2);
    }
}
