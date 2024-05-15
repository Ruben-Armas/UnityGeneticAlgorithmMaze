using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------
// Definición del Individuo
//  Definimos un individuo como una secuencia de movimientos
//  (arriba, abajo, izquierda, derecha) para navegar por el laberinto.

// Esta clase representará cada posible solución (camino) en nuestra población.
//-------------------------------------------------------
public class Individual {
    public List<Vector2Int> path;
    public float fitness;
    private int width;
    private int height;

    public Individual(int width, int height) {
        this.width = width;
        this.height = height;
        path = new List<Vector2Int>();
        GenerateRandomPath();
    }

    // Genera un camino aleatorio.
    void GenerateRandomPath() {
        path.Add(new Vector2Int(0, 0));
        System.Random rand = new System.Random();

        for (int i = 0; i < width + height - 2; i++) {
            Vector2Int currentPos = path[path.Count - 1];
            List<Vector2Int> possibleMoves = new List<Vector2Int>();

            if (currentPos.x < width - 1) possibleMoves.Add(new Vector2Int(currentPos.x + 1, currentPos.y));
            if (currentPos.y < height - 1) possibleMoves.Add(new Vector2Int(currentPos.x, currentPos.y + 1));

            path.Add(possibleMoves[rand.Next(possibleMoves.Count)]);
        }
    }

    // Calcula la aptitud del camino.
    public void EvaluateFitness(bool[,] maze) {
        int goalX = maze.GetLength(0) - 1;
        int goalY = maze.GetLength(1) - 1;
        Vector2Int goalPos = new Vector2Int(goalX, goalY);

        fitness = 0;
        foreach (Vector2Int step in path) {
            if (maze[step.x, step.y]) break;
            if (step == goalPos) {
                fitness = 1.0f;
                return;
            }
            fitness += 1.0f / path.Count;
        }
    }

    // Mezcla dos caminos para crear un nuevo camino.
    public Individual Crossover(Individual other) {
        Individual child = new Individual(width, height);
        child.path.Clear();

        for (int i = 0; i < path.Count; i++) {
            if (Random.value > 0.5f) {
                child.path.Add(this.path[i]);
            }
            else {
                child.path.Add(other.path[i]);
            }
        }

        return child;
    }

    // Aplica mutaciones aleatorias al camino.
    public void Mutate(float mutationRate) {
        System.Random rand = new System.Random();

        for (int i = 0; i < path.Count; i++) {
            if (Random.value < mutationRate) {
                Vector2Int currentPos = path[i];
                List<Vector2Int> possibleMoves = new List<Vector2Int>();

                if (currentPos.x > 0) possibleMoves.Add(new Vector2Int(currentPos.x - 1, currentPos.y));
                if (currentPos.x < width - 1) possibleMoves.Add(new Vector2Int(currentPos.x + 1, currentPos.y));
                if (currentPos.y > 0) possibleMoves.Add(new Vector2Int(currentPos.x, currentPos.y - 1));
                if (currentPos.y < height - 1) possibleMoves.Add(new Vector2Int(currentPos.x, currentPos.y + 1));

                path[i] = possibleMoves[rand.Next(possibleMoves.Count)];
            }
        }
    }
}