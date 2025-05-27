using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safetynet : MonoBehaviour
{
    public bool isActive;

    public void ActiveSafetynet()
    {
        gameObject.SetActive(true);
        isActive = true;

        StopAllCoroutines();
        StartCoroutine(FloatSafetynet());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(DropSafetynet());
    }

    IEnumerator FloatSafetynet()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(0, 2, 0);

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DropSafetynet()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition - new Vector3(0, 2, 0);

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
        isActive = false;
    }
}
