using UnityEngine;
using System.Collections.Generic;

public class Level4MazeGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject coinPrefab;
    public GameObject enemyPrefab;
    public Transform parentObject;
    public Vector2 cellSize = new Vector2(2f, 2f); // Increased for better alignment
    
    private int[,] mazeLayout = new int[,]
    {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 1, 0, 0, 0, 0, 1},
        {1, 0, 1, 0, 1, 0, 1, 1, 0, 1},
        {1, 0, 1, 0, 0, 0, 0, 1, 0, 1},
        {1, 0, 1, 1, 1, 1, 0, 1, 0, 1},
        {1, 0, 0, 0, 0, 1, 0, 1, 0, 1},
        {1, 1, 1, 1, 0, 1, 0, 1, 0, 1},
        {1, 0, 0, 1, 0, 0, 0, 0, 0, 1},
        {1, 0, 1, 1, 1, 1, 1, 1, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1} // Defined boundary walls
    };

    private List<Vector2> coinPositions = new List<Vector2>
    {
        new Vector2(1, 1), new Vector2(3, 3), new Vector2(6, 2), new Vector2(8, 7)
    };
    
    private List<Vector2> enemyPositions = new List<Vector2>
    {
        new Vector2(2, 4), new Vector2(5, 6), new Vector2(7, 2)
    };

    void Start()
    {
        GenerateMaze();
        SpawnCoins();
        SpawnEnemies();
    }

    void GenerateMaze()
    {
        int rows = mazeLayout.GetLength(0);
        int cols = mazeLayout.GetLength(1);
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (mazeLayout[i, j] == 1) // 1 represents walls
                {
                    Vector3 position = new Vector3(j * cellSize.x, 0, -i * cellSize.y);
                    GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity, parentObject);
                    wall.transform.localScale = new Vector3(cellSize.x, wall.transform.localScale.y, cellSize.y);
                }
            }
        }
    }

    void SpawnCoins()
    {
        foreach (Vector2 pos in coinPositions)
        {
            Vector3 position = new Vector3(pos.x * cellSize.x, 0.5f, -pos.y * cellSize.y);
            Instantiate(coinPrefab, position, Quaternion.identity, parentObject);
        }
    }

    void SpawnEnemies()
    {
        foreach (Vector2 pos in enemyPositions)
        {
            Vector3 position = new Vector3(pos.x * cellSize.x, 0.5f, -pos.y * cellSize.y);
            Instantiate(enemyPrefab, position, Quaternion.identity, parentObject);
        }
    }
}
