using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------
// Generación aleatoria del laberinto utilizando una matriz de celdas.
//  Con un ancho y alto definidos
//-------------------------------------------------------
public class MazeGenerator : MonoBehaviour {
    public int width;
    public int height;
    public GameObject wallPrefab;
    private bool[,] maze;

    void Start() {
        GenerateMaze();
        DrawMaze();
    }

    // Crea un laberinto simple con paredes aleatorias.
    void GenerateMaze() {
        maze = new bool[width, height];
        System.Random rand = new System.Random();

        // Simple maze generation using random walls
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                maze[x, y] = (rand.Next(0, 100) < 20); // 20% chance of being a wall
            }
        }

        // Ensure start and end points are open
        maze[0, 0] = false;
        maze[width - 1, height - 1] = false;
    }

    // Dibuja el laberinto en la escena utilizando wallPrefab
    void DrawMaze() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (maze[x, y]) {
                    Instantiate(wallPrefab, new Vector3(x, 0, y), Quaternion.identity);
                }
            }
        }
    }

    public bool[,] GetMaze() {
        return maze;
    }
}