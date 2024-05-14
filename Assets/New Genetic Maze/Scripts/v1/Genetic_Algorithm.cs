using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------
// Evaluación de la Aptitud
//  Evaluamos la aptitud de un individuo basado en la distancia al objetivo.
//-------------------------------------------------------
/*
public class Genetic_Algorithm : MonoBehaviour {
    public MazeGenerator mazeGenerator;
    public int populationSize = 100;
    public int geneLength = 50;
    public float mutationRate = 0.01f;
    public int generations = 100;
    private List<Individual> population;

    void Start() {
        population = new List<Individual>();

        for (int i = 0; i < populationSize; i++) {
            population.Add(new Individual(geneLength));
        }

        StartCoroutine(Evolve());
    }

    IEnumerator Evolve() {
        for (int generation = 0; generation < generations; generation++) {
            EvaluateFitness();

            List<Individual> newPopulation = new List<Individual>();

            for (int i = 0; i < populationSize; i += 2) {
                Individual parent1 = SelectParent();
                Individual parent2 = SelectParent();

                Individual child1, child2;
                Crossover(parent1, parent2, out child1, out child2);

                Mutate(child1);
                Mutate(child2);

                newPopulation.Add(child1);
                newPopulation.Add(child2);
            }

            population = newPopulation;

            yield return null;
        }

        Individual bestIndividual = GetBestIndividual();
        Debug.Log("Best fitness: " + bestIndividual.fitness);
    }

    void EvaluateFitness() {
        foreach (Individual individual in population) {
            individual.fitness = GetFitness(individual);
        }
    }

    float GetFitness(Individual individual) {
        Vector2Int position = new Vector2Int(0, 0);

        foreach (int gene in individual.genes) {
            switch (gene) {
                case 0: position += Vector2Int.up; break;
                case 1: position += Vector2Int.down; break;
                case 2: position += Vector2Int.left; break;
                case 3: position += Vector2Int.right; break;
            }

            if (position.x < 0 || position.x >= mazeGenerator.width || position.y < 0 || position.y >= mazeGenerator.height ||
                mazeGenerator.maze[position.x, position.y] == 1) {
                return float.MaxValue; // hit a wall or out of bounds
            }
        }

        return Vector2Int.Distance(position, new Vector2Int(mazeGenerator.width - 1, mazeGenerator.height - 1));
    }

    Individual SelectParent() {
        float totalFitness = 0;
        foreach (Individual individual in population) {
            totalFitness += individual.fitness;
        }

        float randomPoint = Random.value * totalFitness;
        float cumulativeFitness = 0;

        foreach (Individual individual in population) {
            cumulativeFitness += individual.fitness;
            if (cumulativeFitness >= randomPoint) {
                return individual;
            }
        }

        return population[0];
    }

    void Crossover(Individual parent1, Individual parent2, out Individual child1, out Individual child2) {
        child1 = new Individual(geneLength);
        child2 = new Individual(geneLength);

        int crossoverPoint = Random.Range(0, geneLength);

        for (int i = 0; i < geneLength; i++) {
            if (i < crossoverPoint) {
                child1.genes[i] = parent1.genes[i];
                child2.genes[i] = parent2.genes[i];
            }
            else {
                child1.genes[i] = parent2.genes[i];
                child2.genes[i] = parent1.genes[i];
            }
        }
    }

    void Mutate(Individual individual) {
        for (int i = 0; i < geneLength; i++) {
            if (Random.value < mutationRate) {
                individual.genes[i] = Random.Range(0, 4);
            }
        }
    }

    Individual GetBestIndividual() {
        Individual best = population[0];

        foreach (Individual individual in population) {
            if (individual.fitness < best.fitness) {
                best = individual;
            }
        }

        return best;
    }
}
*/