using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Breakout
{
    public class GameManager : MonoBehaviour
    {
        [Header("게임오브젝트 초기 설정")]
        public GameObject Ball;
        public GameObject Brick;
        public List<BrickEntry> Bricks;
        public Transform BrickContainer;
        public Transform BallContainer;
        public BallController BaseBallController;
        public PaddleController PaddleController;

        [Header("UI 설정")]
        public TextMeshProUGUI CurrentScoreText;
        public TextMeshProUGUI TotalScoreText;
        public GameObject GameOverUI;
        public Button ButtonOK;
        public Button ButtonRetry;
        public List<GameObject> Lifes;

        private int score;
        private int currentLife = 3;
        private int currentBalls = 1;
        private int rowMaxCount = 7;
        private int columnMaxCount = 4;
        private int maxSpecialBrickPerRow = 3;
        private float brickSpacingX = 4;
        private float brickSpacingY = 2;
        private float initialSpawnY = 6.5f;
        private float newSpawnY = 15;
        private float activeBrickCount;
        private float[] blockWeights = new float[9]
        {
            52f, 6f, 6f, 6f, 6f, 6f, 6f, 6f, 6f
        };
        private List<GameObject> activeBricks = new ();
        private List<GameObject> brickItems = new ();
        private Dictionary<BrickType, GameObject> brickPrefabs = new();
        private Color32[] brickMainColors = { GameColors.Salmon04, GameColors.Yellow04, GameColors.Olive04, GameColors.SkyBlue04 };
        private Color32[] brickSubColors = { GameColors.Salmon08, GameColors.Yellow08, GameColors.Olive08, GameColors.SkyBlue08 };

        private void Awake()
        {
            foreach(var entry in Bricks)
            {
                if (!brickPrefabs.ContainsKey(entry.type))
                {
                    brickPrefabs.Add(entry.type, entry.prefab);
                }
            }

            InitializeBricks(initialSpawnY);

            BaseBallController.OnPointScored.AddListener(() => UpdateUI());
            BaseBallController.OnBallOvered.AddListener(() => CheckGameOver());
            PaddleController.OnHeartGained.AddListener(() => UpdateLifeUI(++currentLife));
            PaddleController.OnBallGained.AddListener(() => AddBaseBall());
            ButtonOK.onClick.AddListener(() => Debug.Log("OK"));
            ButtonRetry.onClick.AddListener(() => Retry());

            activeBrickCount = rowMaxCount * columnMaxCount;
        }

        private void Update()
        {
            if (activeBrickCount == 0)
            {
                StartCoroutine(SpawnNewBlockWave());

                activeBrickCount = rowMaxCount * columnMaxCount;
            }
        }

        private void AddBaseBall()
        {
            currentBalls++;
            GameObject ball = Instantiate(Ball, new Vector3(PaddleController.GetPaddleXPosition(), -6.861f, 0), Quaternion.identity, BallContainer);
            BallController ballController = ball.GetComponent<BallController>();
            ballController.OnPointScored.AddListener(() => UpdateUI());
            ballController.OnBallOvered.AddListener(() => CheckGameOver());
        }

        private void InitializeBricks(float startY)
        {
            foreach(var brick in activeBricks)
            {
                Destroy(brick);
            }

            foreach(var item in brickItems)
            {
                Destroy(item);
            }

            activeBricks.Clear();
            brickItems.Clear();
            System.Random rand = new();
            int colorIndex = 0;

            for (int col = 0; col < columnMaxCount; col++)
            {
                int specialCount = 0;

                for (int row = 0; row < rowMaxCount; row++)
                {
                    float x = -((rowMaxCount - 1) * brickSpacingX) / 2f + row * brickSpacingX;
                    float y = startY - col * brickSpacingY;
                    BrickType type;

                    if(specialCount >= maxSpecialBrickPerRow)
                    {
                        type = BrickType.Basic;
                    }
                    else
                    {
                        type = GetRandomBlockType(rand);

                        if (type != BrickType.Basic)
                        {
                            specialCount++;
                        }
                    }

                    GameObject brick = Instantiate(brickPrefabs[type], new Vector3(x, y, 0), Quaternion.identity, BrickContainer);
                    brick.GetComponent<Brick>().SetColors(brickMainColors[colorIndex % brickMainColors.Length], brickSubColors[colorIndex % brickSubColors.Length]);
                    activeBricks.Add(brick);

                    if(brick.GetComponent<ItemBrick>() != null)
                    {
                        brickItems.Add(brick.GetComponent<ItemBrick>().GetItemObject());
                    }
                }

                colorIndex++;
            }
        }

        private BrickType GetRandomBlockType(System.Random rand)
        {
            float randomValue = (float)(rand.NextDouble() * 100f);
            float cumulative = 0f;

            for (int i = 0; i < blockWeights.Length; i++)
            {
                cumulative += blockWeights[i];

                if (randomValue < cumulative)
                {
                    return (BrickType)i;
                }
            }

            return BrickType.Basic;
        }

        IEnumerator SpawnNewBlockWave()
        {
            InitializeBricks(initialSpawnY);

            List<Vector3> startPositions = new();
            List<Vector3> targetPositions = new();

            foreach (var brick in activeBricks)
            {
                Vector3 startPos = new Vector3(brick.transform.position.x, brick.transform.position.y + newSpawnY, brick.transform.position.z);

                startPositions.Add(startPos);
                targetPositions.Add(brick.transform.position);

                brick.transform.position = startPos;
                brick.SetActive(true);
            }

            float duration = 0.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                for (int i = 0; i < activeBricks.Count; i++)
                {
                    if (activeBricks[i] != null)
                    {
                        activeBricks[i].transform.position = Vector3.Lerp(startPositions[i], targetPositions[i], elapsed / duration);
                    }
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            for (int i = 0; i < activeBricks.Count; i++)
            {
                if(activeBricks[i] != null)
                {
                    activeBricks[i].transform.position = targetPositions[i];
                }
            }
        }

        private void UpdateUI()
        {
            score += 50;
            CurrentScoreText.text = score.ToString();

            activeBrickCount--;
        }

        private void UpdateLifeUI(int life)
        {
            if(life > 3)
            {
                life = 3;
            }

            if(life < 0)
            {
                life = 0;
            }

            for (int i=0; i<life; i++)
            {
                Lifes[i].SetActive(true);
            }

            for(int i=life; i<Lifes.Capacity; i++)
            {
                Lifes[i].SetActive(false);
            }
        }

        private void CheckGameOver()
        {
            if(--currentBalls <= 0)
            {
                UpdateLifeUI(--currentLife);

                if (currentLife < 0)
                {
                    GameOver();
                }
                else
                {
                    AddBaseBall();
                }
            }
        }

        private void GameOver()
        {
            GameOverUI.SetActive(true);
            StopAllCoroutines();
            PaddleController.CanMove = false;
            TotalScoreText.text = score.ToString();
        }

        private void Retry()
        {
            GameOverUI.SetActive(false);

            InitializeBricks(initialSpawnY);

            PaddleController.CanMove = true;

            activeBrickCount = rowMaxCount * columnMaxCount;
            currentLife = 3;
            currentBalls = 0;
            score = 0;

            CurrentScoreText.text = score.ToString();
            UpdateLifeUI(currentLife);
            AddBaseBall();
        }
    }
}

[System.Serializable]
public class BrickEntry
{
    public BrickType type;
    public GameObject prefab;
}
