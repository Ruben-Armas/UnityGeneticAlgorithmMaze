using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm_v3 : MonoBehaviour {
    // Prefab para los muros del laberinto
    [SerializeField] private GameObject wallPrefab;

    // Tamaño de la población
    [SerializeField] private int populationSize = 100;

    // Dimensiones del laberinto
    [SerializeField] private int mazeWidth = 10;
    [SerializeField] private int mazeHeight = 10;

    // Tasa de mutación y número máximo de generaciones
    [SerializeField] private float mutationRate = 0.01f;
    [SerializeField] private int maxGenerations = 100;

    // Lista para almacenar la población
    private List<Individual_v3> population;

    void Start() {
        // Inicializar la población al comienzo
        InitializePopulation();

        // Comenzar la evolución del algoritmo genético
        StartCoroutine(Evolve());
    }

    void InitializePopulation() {
        // Crear una nueva lista para la población
        population = new List<Individual_v3>();

        // Añadir individuos aleatorios a la población
        for (int i = 0; i < populationSize; i++) {
            population.Add(new Individual_v3(mazeWidth, mazeHeight));
        }
    }

    IEnumerator Evolve() {
        // Evolucionar durante un número máximo de generaciones
        for (int generation = 0; generation < maxGenerations; generation++) {
            // Evaluar la aptitud de cada individuo en la población
            EvaluatePopulation();

            // Crear una nueva población para la siguiente generación
            List<Individual_v3> newPopulation = new List<Individual_v3>();

            // Seleccionar padres y generar descendientes mediante crossover y mutación
            for (int i = 0; i < populationSize; i++) {
                Individual_v3 parent1 = SelectParent();
                Individual_v3 parent2 = SelectParent();
                Individual_v3 offspring = Crossover(parent1, parent2);
                offspring.Mutate(mutationRate);
                newPopulation.Add(offspring);
            }

            // Reemplazar la población antigua con la nueva
            population = newPopulation;
            yield return null;
        }

        // Mostrar el mejor laberinto encontrado
        DisplayMaze(GetBestIndividual());
    }

    void EvaluatePopulation() {
        // Calcular la aptitud de cada individuo y ordenar la población por aptitud
        foreach (Individual_v3 Individual_v3 in population) {
            Individual_v3.CalculateFitness();
        }
        population.Sort((x, y) => x.Fitness.CompareTo(y.Fitness));
    }

    Individual_v3 SelectParent() {
        // Selección por torneo para elegir padres
        int tournamentSize = 5;
        List<Individual_v3> tournament = new List<Individual_v3>();

        for (int i = 0; i < tournamentSize; i++) {
            int randomIndex = Random.Range(0, populationSize);
            tournament.Add(population[randomIndex]);
        }

        tournament.Sort((x, y) => x.Fitness.CompareTo(y.Fitness));
        return tournament[0];
    }

    Individual_v3 Crossover(Individual_v3 parent1, Individual_v3 parent2) {
        // Cruzar dos padres para generar un descendiente
        Individual_v3 offspring = new Individual_v3(mazeWidth, mazeHeight);

        for (int i = 0; i < mazeWidth * mazeHeight; i++) {
            offspring.Genes[i] = (Random.value > 0.5f) ? parent1.Genes[i] : parent2.Genes[i];
        }

        return offspring;
    }

    Individual_v3 GetBestIndividual() {
        // Obtener el mejor individuo de la población actual
        return population[0];
    }

    void DisplayMaze(Individual_v3 bestIndividual) {
        // Destruir el laberinto anterior
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        // Crear el laberinto basado en los genes del mejor individuo
        for (int y = 0; y < mazeHeight; y++) {
            for (int x = 0; x < mazeWidth; x++) {
                if (bestIndividual.Genes[y * mazeWidth + x] == 1) {
                    Instantiate(wallPrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                }
            }
        }
    }
}

public class Individual_v3 {
    public int[] Genes { get; private set; }
    public float Fitness { get; private set; }
    private int mazeWidth;
    private int mazeHeight;

    public Individual_v3(int mazeWidth, int mazeHeight) {
        this.mazeWidth = mazeWidth;
        this.mazeHeight = mazeHeight;
        Genes = new int[mazeWidth * mazeHeight];

        for (int i = 0; i < Genes.Length; i++) {
            Genes[i] = Random.Range(0, 2); // 0 para espacio vacío, 1 para muro
        }
    }

    public void CalculateFitness() {
        // Función de aptitud simple para minimizar el número de muros
        Fitness = 0;
        for (int i = 0; i < Genes.Length; i++) {
            Fitness += Genes[i];
        }
    }

    public void Mutate(float mutationRate) {
        // Aplicar mutación a los genes
        for (int i = 0; i < Genes.Length; i++) {
            if (Random.value < mutationRate) {
                Genes[i] = 1 - Genes[i];
            }
        }
    }
}
