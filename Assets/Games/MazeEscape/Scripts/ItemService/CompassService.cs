using System.Collections;
using UnityEngine;

public class CompassService : MonoBehaviour
{
    [HideInInspector] public GameObject compassArrow;
    [HideInInspector] public Transform player;
    [HideInInspector] public Vector3 exit;

    public void Show(float duration)
    {
        StartCoroutine(CompassRoutine(duration));
    }

    private IEnumerator CompassRoutine(float duration)
    {
        compassArrow.SetActive(true);

        float timer = 0f;
        while (timer < duration)
        {
            Vector3 toExit = (exit - player.position).normalized;
            toExit.y = 0f;

            if (toExit != Vector3.zero)
            {
                float angle = Mathf.Atan2(toExit.x, toExit.z) * Mathf.Rad2Deg;

                compassArrow.transform.rotation = Quaternion.Euler(90f, 0f, -angle);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        compassArrow.SetActive(false);
    }
}
