using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("게임오브젝트 초기 설정")]
    public Transform PipePoolParent;
    public GameObject PipePrefab;
    public Bird bird;
    
    [Header("UI 설정")]
    public GameObject GameOverUI;
    public TextMeshProUGUI CurrentScoreText;
    public TextMeshProUGUI FinalScoreText;
    public Button ButtonOK;
    public Button ButtonRetry;

    private int poolSize = 10;
    private int score = 0;
    private float spawnInterval = 1.5f;
    private float jumpForce = 12;
    private Vector3 spawnPoint = new(10, 0, 0);
    private Queue<GameObject> pool = new();
    private List<Pipe> activePipes = new();

    private void Awake()
    {
        InitializePool();
        SpawnInitialPipes();
        StartCoroutine(SpawnPipes());

        bird.OnTrigger.AddListener(() => GameOver());
        ButtonOK.onClick.AddListener(() => Debug.Log("OK"));
        ButtonRetry.onClick.AddListener(() => Retry());
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
            pipeScript.OnPointScored.AddListener(() => {
                score++;
                UpdateUI();
            });
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
            activePipes.Add(pipeScript);
            return pipeObject;
        }
        else
        {
            var pipeObject = Instantiate(PipePrefab, PipePoolParent);
            var pipeScript = pipeObject.GetComponent<Pipe>();
            pipeScript.OnBecameInvisible.AddListener(() => ReleasePipe(pipeObject));
            pipeScript.OnPointScored.AddListener(() => {
                score++;
                UpdateUI();
            });
            activePipes.Add(pipeScript);
            return pipeObject;
        }
    }

    public void ReleasePipe(GameObject pipeObject)
    {
        pipeObject.SetActive(false);
        pool.Enqueue(pipeObject);

        Pipe pipeToRemove = null;

        foreach(var pipe in activePipes)
        {
            if (pipe.gameObject == pipeObject)
            {
                pipeToRemove = pipe;
                break;
            }
        }
        
        if (pipeToRemove != null)
        {
            activePipes.Remove(pipeToRemove);
        }
    }

    IEnumerator SpawnPipes()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnPipe();
        }
    }

    private void SpawnPipe()
    {
        var pipe = GetPipe();
        pipe.transform.position = spawnPoint;
    }

    private void UpdateUI()
    {
        CurrentScoreText.text = score.ToString();
    }

    private void GameOver()
    {
        bird.enabled = false;
        bird.PlayAnimation("Hit");

        foreach(var pipe in activePipes)
        {
            pipe.enabled = false;
        }

        StopAllCoroutines();

        CurrentScoreText.gameObject.SetActive(false);
        GameOverUI.SetActive(true);
        FinalScoreText.text = CurrentScoreText.text;
    }

    private void Retry()
    {
        score = 0;
        CurrentScoreText.text = "0";
        CurrentScoreText.gameObject.SetActive(true);
        GameOverUI.SetActive(false);

        bird.enabled = true;
        bird.Reset();

        List<Pipe> pipesToRelease = new List<Pipe>(activePipes);

        foreach (var pipe in pipesToRelease)
        {
            pipe.enabled = true;
            ReleasePipe(pipe.gameObject);
        }

        activePipes.Clear();

        InitializePool();
        SpawnInitialPipes();
        StartCoroutine(SpawnPipes());
    }
}
