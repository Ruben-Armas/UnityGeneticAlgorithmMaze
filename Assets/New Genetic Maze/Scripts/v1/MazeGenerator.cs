using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------
// Generación aleatoria del laberinto
//  Con un ancho y alto definidos
//-------------------------------------------------------
/*
public class MazeGenerator : MonoBehaviour {
    public int width = 10;
    public int height = 10;
    public GameObject wallPrefab;
    public GameObject floorPrefab;

    private int[,] maze;

    void Start() {
        GenerateMaze();
        DrawMaze();
    }

    void GenerateMaze() {
        maze = new int[width, height];

        // Simple maze generation algorithm (e.g., randomized Prim's algorithm)
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                maze[x, y] = (Random.value < 0.2f) ? 1 : 0; // 1 for wall, 0 for floor
            }
        }

        // Ensure start and end points are open
        maze[0, 0] = 0;
        maze[width - 1, height - 1] = 0;
    }

    void DrawMaze() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                GameObject prefab = (maze[x, y] == 1) ? wallPrefab : floorPrefab;
                Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity);
            }
        }
    }
}
*/