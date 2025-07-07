using UnityEngine;
using System.Collections;

public class MiniMapService : MonoBehaviour
{
    public GameObject miniMapUI;
    public Camera miniMapCamera;

    public void Show(float duration)
    {
        StartCoroutine(MiniMapRoutine(duration));
    }

    private IEnumerator MiniMapRoutine(float duration)
    {
        miniMapUI.SetActive(true);
        miniMapCamera.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        miniMapUI.SetActive(false);
        miniMapCamera.gameObject.SetActive(false);
    }
}
