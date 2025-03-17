// using UnityEngine;
// using System.Collections;

// //<summary>
// //Game object, that creates maze and instantiates it in scene
// //</summary>
// public class MazeSpawner : MonoBehaviour {
// 	public enum MazeGenerationAlgorithm{
// 		PureRecursive,
// 		RecursiveTree,
// 		RandomTree,
// 		OldestTree,
// 		RecursiveDivision,
// 	}

// 	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
// 	public bool FullRandom = false;
// 	public int RandomSeed = 12345;
// 	public GameObject Floor = null;
// 	public GameObject Wall = null;
// 	public GameObject Pillar = null;
// 	public int Rows = 5;
// 	public int Columns = 5;
// 	public float CellWidth = 5;
// 	public float CellHeight = 5;
// 	public bool AddGaps = true;
// 	public GameObject GoalPrefab = null;

// 	private BasicMazeGenerator mMazeGenerator = null;

// 	void Start () {
// 		if (!FullRandom) {
// 			Random.seed = RandomSeed;
// 		}
// 		switch (Algorithm) {
// 		case MazeGenerationAlgorithm.PureRecursive:
// 			mMazeGenerator = new RecursiveMazeGenerator (Rows, Columns);
// 			break;
// 		case MazeGenerationAlgorithm.RecursiveTree:
// 			mMazeGenerator = new RecursiveTreeMazeGenerator (Rows, Columns);
// 			break;
// 		case MazeGenerationAlgorithm.RandomTree:
// 			mMazeGenerator = new RandomTreeMazeGenerator (Rows, Columns);
// 			break;
// 		case MazeGenerationAlgorithm.OldestTree:
// 			mMazeGenerator = new OldestTreeMazeGenerator (Rows, Columns);
// 			break;
// 		case MazeGenerationAlgorithm.RecursiveDivision:
// 			mMazeGenerator = new DivisionMazeGenerator (Rows, Columns);
// 			break;
// 		}
// 		mMazeGenerator.GenerateMaze ();
// 		Color wallColor = new Color(Random.value, Random.value, Random.value); // Generate one random color for all walls

// 		for (int row = 0; row < Rows; row++) {
// 			for(int column = 0; column < Columns; column++){
// 				float x = column*(CellWidth+(AddGaps?.2f:0));
// 				float z = row*(CellHeight+(AddGaps?.2f:0));
// 				MazeCell cell = mMazeGenerator.GetMazeCell(row,column);
// 				GameObject tmp;
// 				tmp = Instantiate(Floor,new Vector3(x,0,z), Quaternion.Euler(0,0,0)) as GameObject;
// 				tmp.transform.parent = transform;
// 				if(cell.WallRight){
// 					tmp = Instantiate(Wall,new Vector3(x+CellWidth/2,0,z)+Wall.transform.position,Quaternion.Euler(0,90,0)) as GameObject;// right
// 					tmp.GetComponent<Renderer>().material.color = wallColor;

// 					tmp.transform.parent = transform;
// 				}
// 				if(cell.WallFront){
// 					tmp = Instantiate(Wall,new Vector3(x,0,z+CellHeight/2)+Wall.transform.position,Quaternion.Euler(0,0,0)) as GameObject;// front
// 					tmp.GetComponent<Renderer>().material.color = wallColor;

// 					tmp.transform.parent = transform;
// 				}
// 				if(cell.WallLeft){
// 					tmp = Instantiate(Wall,new Vector3(x-CellWidth/2,0,z)+Wall.transform.position,Quaternion.Euler(0,270,0)) as GameObject;// left
// 					tmp.GetComponent<Renderer>().material.color = wallColor;

// 					tmp.transform.parent = transform;
// 				}
// 				if(cell.WallBack){
// 					tmp = Instantiate(Wall,new Vector3(x,0,z-CellHeight/2)+Wall.transform.position,Quaternion.Euler(0,180,0)) as GameObject;// back
// 					tmp.GetComponent<Renderer>().material.color = wallColor;

// 					tmp.transform.parent = transform;
// 				}
// 				if(cell.IsGoal && GoalPrefab != null){
// 					tmp = Instantiate(GoalPrefab,new Vector3(x,1,z), Quaternion.Euler(0,0,0)) as GameObject;
// 					tmp.transform.parent = transform;
// 				}
// 			}
// 		}
// 		if(Pillar != null){
// 			for (int row = 0; row < Rows+1; row++) {
// 				for (int column = 0; column < Columns+1; column++) {
// 					float x = column*(CellWidth+(AddGaps?.2f:0));
// 					float z = row*(CellHeight+(AddGaps?.2f:0));
// 					GameObject tmp = Instantiate(Pillar,new Vector3(x-CellWidth/2,0,z-CellHeight/2),Quaternion.identity) as GameObject;
// 					tmp.transform.parent = transform;
// 				}
// 			}
// 		}
// 	}
// // }
// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// //<summary>
// //Game object, that creates maze and instantiates it in scene
// //</summary>
// public class MazeSpawner : MonoBehaviour {
//     public enum MazeGenerationAlgorithm {
//         PureRecursive,
//         RecursiveTree,
//         RandomTree,
//         OldestTree,
//         RecursiveDivision,
//     }

//     public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
//     public bool FullRandom = false;
//     public int RandomSeed = 12345;
//     public GameObject Floor = null;
//     public GameObject Wall = null;
//     public GameObject Pillar = null;
//     public int Rows = 5;
//     public int Columns = 5;
//     public float CellWidth = 5;
//     public float CellHeight = 5;
//     public bool AddGaps = true;
//     public GameObject GoalPrefab = null;
//      public GameObject[] PlayerPrefabs;

//     private BasicMazeGenerator mMazeGenerator = null;
//     private List<Vector3> availablePositions = new List<Vector3>();

//     void Start() {
//         if (!FullRandom) {
//             Random.InitState(RandomSeed); // Updated from deprecated Random.seed
//         }

//         // Initialize the maze generator
//         switch (Algorithm) {
//             case MazeGenerationAlgorithm.PureRecursive:
//                 mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
//                 break;
//             case MazeGenerationAlgorithm.RecursiveTree:
//                 mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
//                 break;
//             case MazeGenerationAlgorithm.RandomTree:
//                 mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
//                 break;
//             case MazeGenerationAlgorithm.OldestTree:
//                 mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
//                 break;
//             case MazeGenerationAlgorithm.RecursiveDivision:
//                 mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
//                 break;
//         }

//         mMazeGenerator.GenerateMaze();

//         // 🔹 Set a fixed Dark Blue color for all walls (#00008B)
//         Color wallColor = Color.HSVToRGB(Random.value, 0.9f, 1f); // ✅ Random vibrant color


//         // Loop through the maze and instantiate objects
//         for (int row = 0; row < Rows; row++) {
//             for (int column = 0; column < Columns; column++) {
//                 float x = column * (CellWidth + (AddGaps ? .2f : 0));
//                 float z = row * (CellHeight + (AddGaps ? .2f : 0));
//                 MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
//                 GameObject tmp;

//                 // Floor
//                 tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.identity);
//                 tmp.transform.parent = transform;
//                 availablePositions.Add(new Vector3(x, 1, z));

//                 // Walls with applied color
//                 if (cell.WallRight) {
//                     tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0));
//                     ApplyWallColor(tmp, wallColor);
//                 }
//                 if (cell.WallFront) {
//                     tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position, Quaternion.identity);
//                     ApplyWallColor(tmp, wallColor);
//                 }
//                 if (cell.WallLeft) {
//                     tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0));
//                     ApplyWallColor(tmp, wallColor);
//                 }
//                 if (cell.WallBack) {
//                     tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0));
//                     ApplyWallColor(tmp, wallColor);
//                 }

//                 // Goal
//                 if (cell.IsGoal && GoalPrefab != null) {
//                     tmp = Instantiate(GoalPrefab, new Vector3(x, 1, z), Quaternion.identity);
//                     tmp.transform.parent = transform;
//                 }
//             }
//         }

//         // Pillars
//         if (Pillar != null) {
//             for (int row = 0; row < Rows + 1; row++) {
//                 for (int column = 0; column < Columns + 1; column++) {
//                     float x = column * (CellWidth + (AddGaps ? .2f : 0));
//                     float z = row * (CellHeight + (AddGaps ? .2f : 0));
//                     GameObject tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2), Quaternion.identity);
//                     tmp.transform.parent = transform;
//                 }
//             }
//             SpawnPlayers();
//         }
//     }
//     void SpawnPlayers() {
//         if (PlayerPrefabs.Length < 6) {
//             Debug.LogError("You need at least 6 different Player Prefabs assigned!");
//             return;
//         }

//         int playerCount = Mathf.Min(6, availablePositions.Count); // Ensure we have enough positions
//         List<int> usedIndices = new List<int>();

//         for (int i = 0; i < playerCount; i++) {
//             int randomIndex;
//             do {
//                 randomIndex = Random.Range(0, availablePositions.Count);
//             } while (usedIndices.Contains(randomIndex)); // Ensure unique spawn positions

//             usedIndices.Add(randomIndex);
//             Vector3 spawnPosition = availablePositions[randomIndex];

//             int randomPrefabIndex = Random.Range(0, PlayerPrefabs.Length);
//             GameObject player = Instantiate(PlayerPrefabs[randomPrefabIndex], spawnPosition, Quaternion.identity);
//             Debug.Log($"Player {i + 1} spawned at {spawnPosition}");
//         }
//     }



//     // ✅ Function to Apply the Same Color & Glow to All Walls
//     void ApplyWallColor(GameObject wall, Color color) {
//         if (wall == null) return; // Prevent errors if instantiation fails

//         Renderer renderer = wall.GetComponent<Renderer>();
//         if (renderer != null) {
//             renderer.material.color = color; // Apply Dark Blue color
//             renderer.material.SetColor("_EmissionColor", color * 2f); // Add glow effect
//             renderer.material.EnableKeyword("_EMISSION");
//         }

//         wall.transform.parent = transform; // Maintain hierarchy
//     }
// // }
// using UnityEngine;
// using UnityEngine.AI;
// using Unity.AI.Navigation; // ✅ Required for NavMeshSurface
// using System.Collections;
// using System.Collections.Generic;

// public class MazeSpawner : MonoBehaviour
// {
//     public enum MazeGenerationAlgorithm
//     {
//         PureRecursive,
//         RecursiveTree,
//         RandomTree,
//         OldestTree,
//         RecursiveDivision,
//     }

//     public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
//     public bool FullRandom = false;
//     public int RandomSeed = 12345;
//     public GameObject Floor = null;
//     public GameObject Wall = null;
//     public GameObject Pillar = null;
//     public int Rows = 5;
//     public int Columns = 5;
//     public float CellWidth = 5f;
//     public float CellHeight = 5f;
//     public bool AddGaps = true;
//     public GameObject GoalPrefab = null;
//     public GameObject[] PlayerPrefabs;

//     private BasicMazeGenerator mMazeGenerator = null;
//     private List<Vector3> availablePositions = new List<Vector3>();
//     private NavMeshSurface navMeshSurface;

//     void Start()
//     {
//         if (!FullRandom) Random.InitState(RandomSeed);

//         GenerateMaze();
//         AutoBakeNavMesh();
//     }

//     void GenerateMaze()
//     {
//         // Select the maze generation algorithm
//         switch (Algorithm)
//         {
//             case MazeGenerationAlgorithm.PureRecursive:
//                 mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
//                 break;
//             case MazeGenerationAlgorithm.RecursiveTree:
//                 mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
//                 break;
//             case MazeGenerationAlgorithm.RandomTree:
//                 mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
//                 break;
//             case MazeGenerationAlgorithm.OldestTree:
//                 mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
//                 break;
//             case MazeGenerationAlgorithm.RecursiveDivision:
//                 mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
//                 break;
//         }

//         mMazeGenerator.GenerateMaze();
//         Color wallColor = Color.HSVToRGB(Random.value, 0.9f, 1f); // Random vibrant color

//         for (int row = 0; row < Rows; row++)
//         {
//             for (int column = 0; column < Columns; column++)
//             {
//                 float x = column * (CellWidth + (AddGaps ? 0.2f : 0));
//                 float z = row * (CellHeight + (AddGaps ? 0.2f : 0));
//                 MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
//                 GameObject tmp;

//                 // Instantiate floor
//                 tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.identity);
//                 tmp.transform.parent = transform;
//                 availablePositions.Add(new Vector3(x, 1, z));

//                 // Walls
//                 if (cell.WallRight)
//                 {
//                     tmp = Instantiate(Wall, new Vector3(x + (CellWidth / 2), 0, z), Quaternion.Euler(0, 90, 0));
//                     ApplyWallColor(tmp, wallColor);
//                 }
//                 if (cell.WallFront)
//                 {
//                     tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2), Quaternion.identity);
//                     ApplyWallColor(tmp, wallColor);
//                 }
//                 if (cell.WallLeft)
//                 {
//                     tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z), Quaternion.Euler(0, 270, 0));
//                     ApplyWallColor(tmp, wallColor);
//                 }
//                 if (cell.WallBack)
//                 {
//                     tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2), Quaternion.Euler(0, 180, 0));
//                     ApplyWallColor(tmp, wallColor);
//                 }

//                 // Goal
//                 if (cell.IsGoal && GoalPrefab != null)
//                 {
//                     tmp = Instantiate(GoalPrefab, new Vector3(x, 1, z), Quaternion.identity);
//                     tmp.transform.parent = transform;
//                 }
//             }
//         }

//         // Pillars
//         if (Pillar != null)
//         {
//             for (int row = 0; row < Rows + 1; row++)
//             {
//                 for (int column = 0; column < Columns + 1; column++)
//                 {
//                     float x = column * (CellWidth + (AddGaps ? .2f : 0));
//                     float z = row * (CellHeight + (AddGaps ? .2f : 0));
//                     GameObject tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2), Quaternion.identity);
//                     tmp.transform.parent = transform;
//                 }
//             }
//         }

//         SpawnPlayers();
//     }

//     void SpawnPlayers()
//     {
//         if (PlayerPrefabs.Length < 6)
//         {
//             Debug.LogError("You need at least 6 different Player Prefabs assigned!");
//             return;
//         }

//         int playerCount = Mathf.Min(6, availablePositions.Count);
//         List<int> usedIndices = new List<int>();

//         for (int i = 0; i < playerCount; i++)
//         {
//             int randomIndex;
//             do
//             {
//                 randomIndex = Random.Range(0, availablePositions.Count);
//             } while (usedIndices.Contains(randomIndex));

//             usedIndices.Add(randomIndex);
//             Vector3 spawnPosition = availablePositions[randomIndex];

//             int randomPrefabIndex = Random.Range(0, PlayerPrefabs.Length);
//             GameObject player = Instantiate(PlayerPrefabs[randomPrefabIndex], spawnPosition, Quaternion.identity);
//             Debug.Log($"Player {i + 1} spawned at {spawnPosition}");
//         }
//     }

//     // ✅ Corrected: Function to Apply Color & Glow to All Walls
//     void ApplyWallColor(GameObject wall, Color color)
//     {
//         if (wall == null) return;

//         Renderer renderer = wall.GetComponent<Renderer>();
//         if (renderer != null)
//         {
//             renderer.material.color = color;
//             renderer.material.SetColor("_EmissionColor", color * 2f);
//             renderer.material.EnableKeyword("_EMISSION");
//         }

//         wall.transform.parent = transform;
//     }

//     // ✅ Auto-Baking NavMesh at Runtime
//     void AutoBakeNavMesh()
//     {
//         navMeshSurface = gameObject.GetComponent<NavMeshSurface>();
//         if (navMeshSurface == null)
//         {
//             navMeshSurface = gameObject.AddComponent<NavMeshSurface>();
//         }

//         navMeshSurface.collectObjects = CollectObjects.Children;
//         navMeshSurface.useGeometry = NavMeshCollectGeometry.RenderMeshes;
//         navMeshSurface.BuildNavMesh(); // Auto-bake NavMesh
        
//         // Auto delete the NavMesh after 60 seconds (adjust as needed)
//         StartCoroutine(DeleteNavMeshAfterTime(360f));
//     }

//     // ✅ Remove the NavMesh after a set time
//     IEnumerator DeleteNavMeshAfterTime(float time)
//     {
//         yield return new WaitForSeconds(time);
//         if (navMeshSurface != null)
//         {
//             navMeshSurface.RemoveData();
//             Destroy(navMeshSurface);
//         }
//     }
// }

using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation; // ✅ Required for NavMeshSurface
using System.Collections;
using System.Collections.Generic;

public class MazeSpawner : MonoBehaviour
{
    public enum MazeGenerationAlgorithm
    {
        PureRecursive,
        RecursiveTree,
        RandomTree,
        OldestTree,
        RecursiveDivision,
    }

    public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
    public bool FullRandom = false;
    public int RandomSeed = 12345;
    public GameObject Floor = null;
    public GameObject Wall = null;
    public GameObject Pillar = null;
    public int Rows = 5;
    public int Columns = 5;
    public float CellWidth = 5f;
    public float CellHeight = 5f;
    public bool AddGaps = true;
    public GameObject GoalPrefab = null;
    public GameObject[] PlayerPrefabs;

    private BasicMazeGenerator mMazeGenerator = null;
    private List<Vector3> availablePositions = new List<Vector3>();
    private NavMeshSurface navMeshSurface;
    List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();

    void Start()
    {
        if (!FullRandom) Random.InitState(RandomSeed);

        GenerateMaze();
        AutoBakeNavMesh();
    }

    void GenerateMaze()
    {
        // Select the maze generation algorithm
        switch (Algorithm)
        {
            case MazeGenerationAlgorithm.PureRecursive:
                mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveTree:
                mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RandomTree:
                mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.OldestTree:
                mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveDivision:
                mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
                break;
        }

        mMazeGenerator.GenerateMaze();
        Color wallColor = Color.HSVToRGB(Random.value, 0.9f, 1f); // Random vibrant color

        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                float x = column * (CellWidth + (AddGaps ? 0.2f : 0));
                float z = row * (CellHeight + (AddGaps ? 0.2f : 0));
                MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
                GameObject tmp;

                // Instantiate floor
                tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.identity);
                tmp.transform.parent = transform;
                availablePositions.Add(new Vector3(x, 1, z));

                // Walls
                if (cell.WallRight)
                {
                    tmp = Instantiate(Wall, new Vector3(x + (CellWidth / 2), 0, z), Quaternion.Euler(0, 90, 0));
                    ApplyWallColor(tmp, wallColor);
                }
                if (cell.WallFront)
                {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2), Quaternion.identity);
                    ApplyWallColor(tmp, wallColor);
                }
                if (cell.WallLeft)
                {
                    tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z), Quaternion.Euler(0, 270, 0));
                    ApplyWallColor(tmp, wallColor);
                }
                if (cell.WallBack)
                {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2), Quaternion.Euler(0, 180, 0));
                    ApplyWallColor(tmp, wallColor);
                }

                // Goal
                if (cell.IsGoal && GoalPrefab != null)
                {
                    tmp = Instantiate(GoalPrefab, new Vector3(x, 1, z), Quaternion.identity);
                    tmp.transform.parent = transform;
                }
            }
        }

        // Pillars
        if (Pillar != null)
        {
            for (int row = 0; row < Rows + 1; row++)
            {
                for (int column = 0; column < Columns + 1; column++)
                {
                    float x = column * (CellWidth + (AddGaps ? .2f : 0));
                    float z = row * (CellHeight + (AddGaps ? .2f : 0));
                    GameObject tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2), Quaternion.identity);
                    tmp.transform.parent = transform;
                }
            }
        }

        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        if (PlayerPrefabs.Length < 6)
        {
            Debug.LogError("You need at least 6 different Player Prefabs assigned!");
            return;
        }

        int playerCount = Mathf.Min(6, availablePositions.Count);
        List<int> usedIndices = new List<int>();

        for (int i = 0; i < playerCount; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, availablePositions.Count);
            } while (usedIndices.Contains(randomIndex));

            usedIndices.Add(randomIndex);
            Vector3 spawnPosition = availablePositions[randomIndex];

            int randomPrefabIndex = Random.Range(0, PlayerPrefabs.Length);
            GameObject player = Instantiate(PlayerPrefabs[randomPrefabIndex], spawnPosition, Quaternion.identity);
            Debug.Log($"Player {i + 1} spawned at {spawnPosition}");
        }
    }

    // ✅ Corrected: Function to Apply Color & Glow to All Walls
    void ApplyWallColor(GameObject wall, Color color)
    {
        if (wall == null) return;

        Renderer renderer = wall.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
            renderer.material.SetColor("_EmissionColor", color * 2f);
            renderer.material.EnableKeyword("_EMISSION");
        }

        wall.transform.parent = transform;
    }

    // ✅ Auto-Baking NavMesh at Runtime
    void AutoBakeNavMesh()
{
    navMeshSurface = gameObject.GetComponent<NavMeshSurface>();
    if (navMeshSurface == null)
    {
        navMeshSurface = gameObject.AddComponent<NavMeshSurface>();
    }

    // ✅ Set only the walkable layers (exclude walls and pillars)
    int walkableLayerMask = ~(1 << LayerMask.NameToLayer("Walls") | 1 << LayerMask.NameToLayer("Pillars"));
    navMeshSurface.layerMask = walkableLayerMask;

    // ✅ Ensure NavMesh auto-bakes within the maze boundaries
    navMeshSurface.collectObjects = CollectObjects.Children;
    navMeshSurface.useGeometry = NavMeshCollectGeometry.RenderMeshes;
    navMeshSurface.BuildNavMesh(); // Auto-bake NavMesh

    // Auto-delete the NavMesh after 360 seconds (optional)
    StartCoroutine(DeleteNavMeshAfterTime(360f));
    
}


    // ✅ Remove the NavMesh after a set time
    IEnumerator DeleteNavMeshAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (navMeshSurface != null)
        {
            navMeshSurface.RemoveData();
            Destroy(navMeshSurface);
        }
    }
}