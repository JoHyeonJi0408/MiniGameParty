using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform PipePoolParent;
    public GameObject PipePrefab;
    public float spawnInterval;
    public Bird bird;
    public float jumpForce;
    private Vector3 spawnPoint = new Vector3(10, 0, 0);
    private int poolSize = 10;
    private Queue<GameObject> pool = new();
    private Queue<Pipe> pipes = new();

    private void Awake()
    {
        InitializePool();
        SpawnInitialPipes();
        StartCoroutine(SpawnPipes());

        bird.OnTrigger.AddListener(() => GameOver());
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

    private void SpawnInitialPipes()
    {
        for (int i = 0; i < 2; i++)
        {
            var pipe = GetPipe();
            pipe.transform.position = spawnPoint + Vector3.left * 15 * i;
            pipe.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bird.SetVelocityY(-jumpForce);
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
        pipes.Dequeue();
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
        pipes.Enqueue(pipe.GetComponent<Pipe>());
        pipe.transform.position = spawnPoint;
    }

    private void GameOver()
    {
        bird.enabled = false;

        while(pipes.Count > 0)
        {
            var pipe = pipes.Peek();
            pipe.enabled = false;
        }

        StopAllCoroutines();
    }
}
