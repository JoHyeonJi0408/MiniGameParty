using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MazeEscape
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("서비스 설정")]
        public MiniMapService MiniMapService;
        public CompassService CompassService;

        [Header("미로 크기")]
        public int width = 21;
        public int height = 21;

        [Header("프리팹 설정")]
        public GameObject wallPrefab;
        public GameObject floorPrefab;
        public GameObject exitPrefab;
        public GameObject playerPrefab;
        public GameObject ObstacleAIPrefab;

        [Header("아이템 설정")]
        public ItemData MiniMapData;
        public ItemData CompassData;

        [Header("UI 설정")]
        public GameObject GameOverUI;
        public TextMeshProUGUI MinuteText;
        public TextMeshProUGUI SecondText;
        public TextMeshProUGUI TotalTimeText;
        public Button ButtonOK;
        public Button ButtonRetry;

        private int[,] maze;
        private float timer = 0f;
        private bool isEnd = false;
        private List<Vector2> itemSpawnPos = new();
        private Transform player;

        private readonly int[] dx = { 0, 0, -2, 2 };
        private readonly int[] dy = { -2, 2, 0, 0 };

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameStart();

            ButtonOK.onClick.AddListener(() => Debug.Log("OK"));
            ButtonRetry.onClick.AddListener(() => Debug.Log("Retry"));
        }

        private void GameStart()
        {
            GenerateMaze();
            BuildMaze();
            SpawnPlayer();
            SpawnExit();
            SpawnItems();
        }

        private void Update()
        {
            if (isEnd)
            {
                return;
            }

            timer += Time.deltaTime;

            int minutes = Mathf.FloorToInt(timer / 60f);
            float seconds = timer % 60f;

            MinuteText.text = $"{minutes:00}";
            SecondText.text = $"{seconds:00.00}";
        }

        private void GenerateMaze()
        {
            maze = new int[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    maze[x, y] = 1;
                }
            }

            DFS(1, 1);
        }

        private void DFS(int x, int y)
        {
            maze[x, y] = 0;

            List<int> dir = new List<int> { 0, 1, 2, 3 };
            Shuffle(dir);

            foreach (int i in dir)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];

                if (IsInMaze(nx, ny) && maze[nx, ny] == 1)
                {
                    maze[x + dx[i] / 2, y + dy[i] / 2] = 0;
                    DFS(nx, ny);
                }
            }
        }

        private void BuildMaze()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (maze[x, y] == 1)
                    {
                        Instantiate(wallPrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                    }
                    else
                    {
                        Instantiate(floorPrefab, new Vector3(x, -0.45f, y), Quaternion.identity, transform);

                        int wallCount = 0;

                        if (maze[x + 1, y] == 1) wallCount++;
                        if (maze[x - 1, y] == 1) wallCount++;
                        if (maze[x, y + 1] == 1) wallCount++;
                        if (maze[x, y - 1] == 1) wallCount++;

                        if (wallCount >= 3) itemSpawnPos.Add(new Vector2(x, y));
                    }
                }
            }
        }

        private void SpawnPlayer()
        {
            var player = Instantiate(playerPrefab, new Vector3(1, 0.1f, 1), Quaternion.identity);
            var arrow = player.transform.GetChild(0).gameObject;

            this.player = player.transform;
            CompassService.compassArrow = arrow;
            CompassService.player = player.transform;
        }

        private void SpawnExit()
        {
            int ex = width - 2;
            int ey = height - 2;

            if (maze[ex, ey] == 1)
            {
                maze[ex, ey] = 0;
            }

            var exitObject = Instantiate(exitPrefab, new Vector3(ex, 0, ey), Quaternion.identity);

            CompassService.exit = new Vector3(ex, 0, ey);
            exitObject.GetComponent<ExitArrive>().OnExit.AddListener(() => GameOver());
        }

        private void SpawnItems()
        {
            int count = itemSpawnPos.Count;
            if (count <= 2) return;

            List<Vector2> middle = itemSpawnPos.GetRange(1, count - 2)
                                                  .OrderBy(_ => Random.value)
                                                  .ToList();

            int half = middle.Count / 2;

            for (int i = 0; i < middle.Count; i++)
            {
                var pos = middle[i];
                var obstacleAI = Instantiate(ObstacleAIPrefab, new(pos.x, 0.1f, pos.y), Quaternion.identity);
                obstacleAI.GetComponent<ObstacleAI>().player = player;

                /*
                if (i < half)
                {
                    Instantiate(MiniMapData.prefab, new(pos.x, -0.3f, pos.y), Quaternion.identity);
                    
                }
                else
                {
                    Instantiate(CompassData.prefab, new(pos.x, -0.3f, pos.y), Quaternion.identity);
                }*/
            }
        }

        private bool IsInMaze(int x, int y)
        {
            return x > 0 && x < width && y > 0 && y < height;
        }

        private void Shuffle(List<int> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int rand = Random.Range(0, i + 1);
                int temp = list[i];
                list[i] = list[rand];
                list[rand] = temp;
            }
        }

        private void GameOver()
        {
            isEnd = true;

            GameOverUI.SetActive(true);

            int minutes = Mathf.FloorToInt(timer / 60f);
            float seconds = timer % 60f;
            TotalTimeText.text = $"{minutes:00}:{seconds:00.00}";
        }
    }
}
