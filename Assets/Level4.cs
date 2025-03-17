using UnityEngine;

public class Level4 : MonoBehaviour
{
    public GameObject labyrinthPrefab;
    public GameObject wallPrefab;
    public GameObject cagePrefab;
    public GameObject environmentPrefab;
    public GameObject floorPrefab;

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        // Instantiate floor
        GameObject floor = Instantiate(floorPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        floor.transform.localScale = new Vector3(1.5f, 1, 1.5f);

        // Instantiate labyrinth with increased complexity
        GameObject labyrinth = Instantiate(labyrinthPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        labyrinth.transform.localScale *= 1.2f;

        // Add walls to increase difficulty
        for (int i = -2; i <= 2; i++)
        {
            Instantiate(wallPrefab, new Vector3(i * 5, 1, -10), Quaternion.identity);
            Instantiate(wallPrefab, new Vector3(i * 5, 1, 10), Quaternion.identity);
        }

        for (int i = -2; i <= 2; i++)
        {
            Instantiate(wallPrefab, new Vector3(-10, 1, i * 5), Quaternion.identity);
            Instantiate(wallPrefab, new Vector3(10, 1, i * 5), Quaternion.identity);
        }

        // Add cage obstacles
        Instantiate(cagePrefab, new Vector3(0, 1, 5), Quaternion.identity);
        Instantiate(cagePrefab, new Vector3(5, 1, 0), Quaternion.identity);
        Instantiate(cagePrefab, new Vector3(-5, 1, -5), Quaternion.identity);

        // Add environment objects for immersion
        Instantiate(environmentPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
