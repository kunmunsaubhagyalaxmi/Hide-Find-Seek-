using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class MazePrefabGenerator : MonoBehaviour
{
    public GameObject wallPrefab; // Assign a Cube prefab in Unity
    private string jsonFileName = "prefab_data.json"; // Ensure it's in StreamingAssets
    public string savePrefabPath = "Assets/Prefabs/NewMazePrefab.prefab"; // Save location

    [System.Serializable]
    public class Wall
    {
        public float[] position;  // Read position as an array [x, y, z]
        public float[] scale;  // Read scale as an array [x, y, z]
    }

    [System.Serializable]
    public class WallList
    {
        public List<Wall> walls;
    }

    void Start()
    {
        GenerateMazePrefab();
    }

    void GenerateMazePrefab()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            WallList wallData = JsonUtility.FromJson<WallList>(json);

            GameObject mazeContainer = new GameObject("NewMaze");

            foreach (var wall in wallData.walls)
            {
                if (wall.position.Length < 3 || wall.scale.Length < 3)
                {
                    Debug.LogError("Invalid position or scale data in JSON");
                    continue;
                }

                Vector3 spawnPosition = new Vector3(wall.position[0], wall.position[1], wall.position[2]);
                Vector3 wallScale = new Vector3(
                    Mathf.Max(wall.scale[0], 0.5f), // Set minimum scale to 0.5
                    Mathf.Max(wall.scale[1], 0.5f),
                    Mathf.Max(wall.scale[2], 0.5f)
                );

                // Skip walls that are at (0,0,0) (Possible invalid objects)
                if (spawnPosition == Vector3.zero)
                {
                    continue;
                }

                Debug.Log($"Wall Position: {spawnPosition}, Scale: {wallScale}");

                GameObject newWall = Instantiate(wallPrefab, spawnPosition, Quaternion.identity);
                newWall.transform.localScale = wallScale;
                newWall.transform.SetParent(mazeContainer.transform);
            }
        }
        else
        {
            Debug.LogError("Prefab data file not found: " + filePath);
        }
    }
}
