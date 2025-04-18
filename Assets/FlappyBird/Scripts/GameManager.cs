using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform PipePoolParent;
    public GameObject PipePrefab;
    public float spawnInterval;
    private Vector3 spawnPoint = new Vector3(10, 0, 0);
    private int poolSize = 10;
    private Queue<GameObject> pool = new();

    private void Awake()
    {
        InitializePool();
        StartCoroutine(SpawnPipes());
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var pipeObject = Instantiate(PipePrefab, PipePoolParent);
            pipeObject.SetActive(false);
            pool.Enqueue(pipeObject);
            var pipeScript = pipeObject.GetComponent<Pipe>();
            pipeScript.OnBecameInvisible.AddListener(() => ReleasePipe(pipeObject));
        }
    }

    public GameObject GetPipe()
    {
        if (pool.Count > 0)
        {
            var pipeObject = pool.Dequeue();
            pipeObject.SetActive(true);
            var pipeScript = pipeObject.GetComponent<Pipe>();
            pipeScript.ResetPipe();
            return pipeObject;
        }
        else
        {
            var pipeObject = Instantiate(PipePrefab, PipePoolParent);
            var pipeScript = pipeObject.GetComponent<Pipe>();
            pipeScript.OnBecameInvisible.AddListener(() => ReleasePipe(pipeObject));
            return pipeObject;
        }
    }

    public void ReleasePipe(GameObject pipeObject)
    {
        pipeObject.SetActive(false);
        pool.Enqueue(pipeObject);
    }

    IEnumerator SpawnPipes()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnPipe();
        }
    }

    void SpawnPipe()
    {
        var pipe = GetPipe();
        pipe.transform.position = spawnPoint;
    }
}
