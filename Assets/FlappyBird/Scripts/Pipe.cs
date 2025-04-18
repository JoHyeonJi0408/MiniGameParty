using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pipe : MonoBehaviour
{
    public UnityEvent OnBecameInvisible;
    public SpriteRenderer topRenderer;
    public SpriteRenderer bottomRenderer;
    private float moveSpeed = 10f;

    private void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

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
    }
}
