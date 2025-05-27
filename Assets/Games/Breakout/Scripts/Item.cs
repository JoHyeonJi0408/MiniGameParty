using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType ItemType;
    public SpriteRenderer Sprite;
    private float dropSpeed = 8;

    private void Update()
    {
        transform.Translate(dropSpeed * Time.deltaTime * Vector3.down);

        if(transform.position.y < -12)
        {
            Destroy(transform.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Paddle"))
        {
            Destroy(transform.gameObject);
        }
    }

    public void SetColor(Color32 color)
    {
        Sprite.color = color;
    }
}
