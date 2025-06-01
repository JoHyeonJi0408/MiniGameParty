using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PaddleController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Safetynet[] Safetynets;
    private float currentSpeed = 0.1f;
    private float defaultSpeed = 0.1f;
    private Vector3 defaultScale;

    [HideInInspector] public bool CanMove = true;
    [HideInInspector] public UnityEvent OnHeartGained;
    [HideInInspector] public UnityEvent OnBallGained;

    private void Awake()
    {
        defaultScale = transform.localScale;
    }

    private void Update()
    {
        if (!CanMove)
        {
            return;
        }

        float currentX = transform.position.x;
        float newX = currentX;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newX -= currentSpeed;
        } else if (Input.GetKey(KeyCode.RightArrow))
        {
            newX += currentSpeed;
        }

        float currentScale = transform.localScale.x;
        float clamX = 14.8f + (1f - currentScale) * 3f;

        newX = Mathf.Clamp(newX, -clamX, clamX);

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    public float GetPaddleXPosition()
    {
        return transform.position.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var Item = collision.gameObject.GetComponent<Item>();

        if (Item != null)
        {
            switch (Item.ItemType)
            {
                case ItemType.Ball:
                    OnBallGained.Invoke();
                    break;
                case ItemType.Bomb:
                    ApplySlowEffect(0.1f, 1);
                    break;
                case ItemType.Heart:
                    OnHeartGained.Invoke();
                    break;
                case ItemType.Minus:
                    ApplyScaleEffect(0.5f, 1, 5);
                    break;
                case ItemType.Plus:
                    ApplyScaleEffect(1.5f, 1, 5);
                    break;
                case ItemType.Safetynet:
                    foreach(var safetynet in Safetynets)
                    {
                        if (!safetynet.isActive)
                        {
                            safetynet.ActiveSafetynet();
                        }
                    }
                    break;
            }
        }
    }

    private void ApplySlowEffect(float slowMultiplier, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(SlowRoutine(slowMultiplier, duration));
        StartCoroutine(SmoothColorChange(duration));
    }
    private void ApplyScaleEffect(float scaleMultiplier, float scaleDuration, float holdDuration)
    {
        StopCoroutine(nameof(ScaleRoutine));
        StartCoroutine(ScaleRoutine(scaleMultiplier, scaleDuration, holdDuration));
    }

    private IEnumerator SlowRoutine(float slowMultiplier, float duration)
    {
        currentSpeed = defaultSpeed * slowMultiplier;
        yield return new WaitForSeconds(duration);
        currentSpeed = defaultSpeed;
    }

    private IEnumerator SmoothColorChange(float duration)
    {
        float halfDuration = duration / 2f;
        float t = 0f;

        while (t < halfDuration)
        {
            t += Time.deltaTime;
            float lerpT = t / halfDuration;
            spriteRenderer.color = Color.Lerp(Color.white, new Color32(50, 50, 50, 250), lerpT);
            yield return null;
        }

        t = 0f;

        while (t < halfDuration)
        {
            t += Time.deltaTime;
            float lerpT = t / halfDuration;
            spriteRenderer.color = Color.Lerp(new Color32(50, 50, 50, 250), Color.white, lerpT);
            yield return null;
        }

        spriteRenderer.color = Color.white;
    }
    private IEnumerator ScaleRoutine(float scaleMultiplier, float scaleDuration, float holdDuration)
    {
        Vector3 targetScale = new Vector3(defaultScale.x * scaleMultiplier, defaultScale.y, defaultScale.z);
        Vector3 originalScale = defaultScale;

        float t = 0f;

        while (t < scaleDuration)
        {
            t += Time.deltaTime;
            float lerpT = t / scaleDuration;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, lerpT);
            yield return null;
        }

        transform.localScale = targetScale;

        yield return new WaitForSeconds(holdDuration);

        t = 0f;
        while (t < scaleDuration)
        {
            t += Time.deltaTime;
            float lerpT = t / scaleDuration;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, lerpT);
            yield return null;
        }

        transform.localScale = originalScale;
    }
}
