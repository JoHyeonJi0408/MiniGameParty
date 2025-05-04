using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed;

    private void FixedUpdate()
    {
        float currentX = transform.position.x;
        float newX = currentX;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newX -= speed;
        } else if (Input.GetKey(KeyCode.RightArrow))
        {
            newX += speed;
        }

        newX = Mathf.Clamp(newX, -14.7f, 14.7f);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
