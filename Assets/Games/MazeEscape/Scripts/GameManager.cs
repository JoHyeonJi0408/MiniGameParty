using UnityEngine;
using System.Collections.Generic;

namespace MazeEscape
{
    public class GameManager : MonoBehaviour
    {
        [Header("미로 크기")]
        public int width = 21;
        public int height = 21;

        [Header("프리팹 설정")]
        public GameObject wallPrefab;
        public GameObject floorPrefab;
        public GameObject exitPrefab;
        public GameObject playerPrefab;

        private int[,] maze;

        private readonly int[] dx = { 0, 0, -2, 2 };
        private readonly int[] dy = { -2, 2, 0, 0 };

        private void Start()
        {
            GenerateMaze();
            BuildMaze();
            SpawnPlayer();
            SpawnExit();
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
                    }
                }
            }
        }

        private void SpawnPlayer()
        {
            Instantiate(playerPrefab, new Vector3(1, 0.1f, 1), Quaternion.identity);
        }

        private void SpawnExit()
        {
            int ex = width - 2;
            int ey = height - 2;

            if (maze[ex, ey] == 1)
            {
                maze[ex, ey] = 0;
            }

            Instantiate(exitPrefab, new Vector3(ex, 0.1f, ey), Quaternion.identity);
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
    }
}
