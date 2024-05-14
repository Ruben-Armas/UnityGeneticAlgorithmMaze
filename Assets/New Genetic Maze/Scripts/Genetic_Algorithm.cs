using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------
// Agente del Algoritmo Genético
//  Que intentará resolver el laberinto utilizando un algoritmo genético.
//-------------------------------------------------------
public class Genetic_Algorithm : MonoBehaviour {
    public MazeGenerator mazeGenerator;
    public int populationSize = 100;
    public float mutationRate = 0.01f;
    public int maxGenerations = 1000;

    private List<Individual> population;
    private bool[,] maze;

    void Start() {
        maze = mazeGenerator.GetMaze();
        InitializePopulation();
        StartCoroutine(Evolve());
    }

    // Crea una población inicial de individuos con caminos aleatorios.
    void InitializePopulation() {
        population = new List<Individual>();
        for (int i = 0; i < populationSize; i++) {
            population.Add(new Individual(maze.GetLength(0), maze.GetLength(1)));
        }
    }

    // Realiza el ciclo del algoritmo genético: evaluación, selección, cruce y mutación.
    IEnumerator Evolve() {
        for (int generation = 0; generation < maxGenerations; generation++) {
            EvaluateFitness();
            population.Sort((a, b) => b.fitness.CompareTo(a.fitness));

            if (population[0].fitness == 1.0f) {
                Debug.Log("Solution found at generation " + generation);
                DrawPath(population[0]);
                yield break;
            }

            List<Individual> newPopulation = new List<Individual>();
            for (int i = 0; i < populationSize; i++) {
                Individual parent1 = SelectParent();
                Individual parent2 = SelectParent();
                Individual child = parent1.Crossover(parent2);
                child.Mutate(mutationRate);
                newPopulation.Add(child);
            }

            population = newPopulation;
            yield return null;
        }

        Debug.Log("No solution found.");
    }

    // Evalúa la aptitud de cada individuo en función de su capacidad para llegar al objetivo.
    void EvaluateFitness() {
        foreach (Individual individual in population) {
            individual.EvaluateFitness(maze);
        }
    }

    // Selecciona un padre para la reproducción basada en su aptitud.
    Individual SelectParent() {
        float totalFitness = 0;
        foreach (Individual individual in population) {
            totalFitness += individual.fitness;
        }

        float randomValue = Random.Range(0, totalFitness);
        float cumulativeFitness = 0;

        foreach (Individual individual in population) {
            cumulativeFitness += individual.fitness;
            if (cumulativeFitness > randomValue) {
                return individual;
            }
        }

        return population[0];
    }

    // Dibuja el camino de la mejor solución encontrada.
    void DrawPath(Individual individual) {
        Vector3 position = new Vector3(0, 0.5f, 0);
        foreach (Vector2Int step in individual.path) {
            position = new Vector3(step.x, 0.5f, step.y);
            Debug.Log("Step: " + position);
            // Visualize the path here, e.g., instantiate a sphere or change color of the ground
        }
    }
}