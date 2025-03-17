using UnityEngine;

public class RandomMazeGenerator : MonoBehaviour
{
    public GameObject wallPrefab;  // Assign a Cube prefab for walls
    public GameObject floorPrefab; // Assign a Plane prefab for the floor
    public int rows = 6;
    public int cols = 6;
    public float cellSize = 5f;
    public float wallHeight = 2f;
    public float wallThickness = 1f;
    public float randomness = 0.5f; // Adjust this to control maze randomness (0 = structured, 1 = chaotic)

    void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        // Create floor
        GameObject floor = Instantiate(floorPrefab, new Vector3((cols * cellSize) / 2, -0.1f, (rows * cellSize) / 2), Quaternion.identity);
        floor.transform.localScale = new Vector3(cols * cellSize / 10, 1, rows * cellSize / 10);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 cellPosition = new Vector3(col * cellSize, 0, row * cellSize);

                // Randomly place walls with some open spaces
                if (Random.value > randomness)
                {
                    CreateWall(cellPosition + new Vector3(cellSize / 2, 0, 0), new Vector3(wallThickness, wallHeight, cellSize)); // Right Wall
                }
                if (Random.value > randomness)
                {
                    CreateWall(cellPosition + new Vector3(0, 0, cellSize / 2), new Vector3(cellSize, wallHeight, wallThickness)); // Front Wall
                }
            }
        }
    }

    void CreateWall(Vector3 position, Vector3 scale)
    {
        GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity);
        wall.transform.localScale = scale;
        wall.transform.parent = transform;
    }
}
