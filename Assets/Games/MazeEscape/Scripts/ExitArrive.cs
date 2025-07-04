using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.Events;

public class ExitArrive : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnExit;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        OnExit?.Invoke();
    }
}
