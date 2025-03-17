using UnityEngine;

public class MazeLevel4Generator : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject groundPrefab;

    private Vector3[] wallPositions = new Vector3[]
    {
        new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(2, 0, 0), new Vector3(3, 0, 0), new Vector3(4, 0, 0), new Vector3(5, 0, 0), new Vector3(6, 0, 0),
        new Vector3(0, 0, 1), new Vector3(6, 0, 1),
        new Vector3(0, 0, 2), new Vector3(2, 0, 2), new Vector3(4, 0, 2), new Vector3(6, 0, 2),
        new Vector3(0, 0, 3), new Vector3(2, 0, 3), new Vector3(4, 0, 3), new Vector3(6, 0, 3),
        new Vector3(0, 0, 4), new Vector3(6, 0, 4),
        new Vector3(0, 0, 5), new Vector3(1, 0, 5), new Vector3(2, 0, 5), new Vector3(3, 0, 5), new Vector3(4, 0, 5), new Vector3(6, 0, 5),
        new Vector3(0, 0, 6), new Vector3(6, 0, 6)
    };

    void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        // Instantiate ground
        GameObject ground = Instantiate(groundPrefab, new Vector3(3, -0.5f, 3), Quaternion.identity);
        ground.transform.localScale = new Vector3(7, 1, 7);

        // Generate walls at specified positions
        foreach (Vector3 pos in wallPositions)
        {
            Instantiate(wallPrefab, pos, Quaternion.identity);
        }
    }
}
