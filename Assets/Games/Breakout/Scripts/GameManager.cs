using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Breakout
{
    public class GameManager : MonoBehaviour
    {
        [Header("게임오브젝트 초기 설정")]
        public GameObject Brick;
        public List<BrickEntry> Bricks;
        public Transform BrickContainer;
        public BallController BallController;

        [Header("UI 설정")]
        public TextMeshProUGUI ScoreText;

        private int score;
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

            BallController.OnPointScored.AddListener(() => UpdateUI());

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

        private void InitializeBricks(float startY)
        {
            activeBricks.Clear();
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
                activeBricks[i].transform.position = targetPositions[i];
            }
        }

        private void UpdateUI()
        {
            score += 50;
            ScoreText.text = score.ToString();

            activeBrickCount--;
        }
    }
}

[System.Serializable]
public class BrickEntry
{
    public BrickType type;
    public GameObject prefab;
}
