using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm_v4 : MonoBehaviour {
    // Prefabs para muros, suelo y camino
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject pathPrefab;

    // Tama�o de la poblaci�n
    [SerializeField] private int populationSize = 100;

    // Dimensiones del laberinto
    [SerializeField] private int mazeWidth = 10;
    [SerializeField] private int mazeHeight = 10;

    // Tasa de mutaci�n y n�mero m�ximo de generaciones
    [SerializeField] private float mutationRate = 0.01f;
    [SerializeField] private int maxGenerations = 100;

    // Lista para almacenar la poblaci�n
    private List<Individual_v4> population;

    void Start() {
        // Inicializar la poblaci�n al comienzo
        InitializePopulation();

        // Comenzar la evoluci�n del algoritmo gen�tico
        StartCoroutine(Evolve());
    }

    void InitializePopulation() {
        // Crear una nueva lista para la poblaci�n
        population = new List<Individual_v4>();

        // A�adir individuos aleatorios a la poblaci�n
        for (int i = 0; i < populationSize; i++) {
            population.Add(new Individual_v4(mazeWidth, mazeHeight));
        }
    }

    IEnumerator Evolve() {
        // Evolucionar durante un n�mero m�ximo de generaciones
        for (int generation = 0; generation < maxGenerations; generation++) {
            // Evaluar la aptitud de cada individuo en la poblaci�n
            EvaluatePopulation();

            // Crear una nueva poblaci�n para la siguiente generaci�n
            List<Individual_v4> newPopulation = new List<Individual_v4>();

            // Seleccionar padres y generar descendientes mediante crossover y mutaci�n
            for (int i = 0; i < populationSize; i++) {
                Individual_v4 parent1 = SelectParent();
                Individual_v4 parent2 = SelectParent();
                Individual_v4 offspring = Crossover(parent1, parent2);
                offspring.Mutate(mutationRate);
                newPopulation.Add(offspring);
            }

            // Reemplazar la poblaci�n antigua con la nueva
            population = newPopulation;
            yield return null;
        }

        // Mostrar el mejor laberinto encontrado
        DisplayMaze(GetBestIndividual());
    }

    void EvaluatePopulation() {
        // Calcular la aptitud de cada individuo y ordenar la poblaci�n por aptitud
        foreach (Individual_v4 Individual_v4 in population) {
            Individual_v4.CalculateFitness();
        }
        population.Sort((x, y) => x.Fitness.CompareTo(y.Fitness));
    }

    Individual_v4 SelectParent() {
        // Selecci�n por torneo para elegir padres
        int tournamentSize = 5;
        List<Individual_v4> tournament = new List<Individual_v4>();

        for (int i = 0; i < tournamentSize; i++) {
            int randomIndex = Random.Range(0, populationSize);
            tournament.Add(population[randomIndex]);
        }

        tournament.Sort((x, y) => x.Fitness.CompareTo(y.Fitness));
        return tournament[0];
    }

    Individual_v4 Crossover(Individual_v4 parent1, Individual_v4 parent2) {
        // Cruzar dos padres para generar un descendiente
        Individual_v4 offspring = new Individual_v4(mazeWidth, mazeHeight);

        for (int i = 0; i < mazeWidth * mazeHeight; i++) {
            offspring.Genes[i] = (Random.value > 0.5f) ? parent1.Genes[i] : parent2.Genes[i];
        }

        return offspring;
    }

    Individual_v4 GetBestIndividual() {
        // Obtener el mejor individuo de la poblaci�n
        return population[0];
    }

    void DisplayMaze(Individual_v4 bestIndividual) {
        // Eliminar laberintos anteriores
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        // Crear el nuevo laberinto basado en los genes del mejor individuo
        for (int y = 0; y < mazeHeight; y++) {
            for (int x = 0; x < mazeWidth; x++) {
                Vector3 position = new Vector3(x, 0, y);
                Instantiate(floorPrefab, position, Quaternion.identity, transform); // Colocar suelo
                if (bestIndividual.Genes[y * mazeWidth + x] == 1) {
                    Instantiate(wallPrefab, position, Quaternion.identity, transform); // Colocar muro
                }
                else {
                    Instantiate(pathPrefab, position, Quaternion.identity, transform); // Colocar camino
                }
            }
        }
    }
}

// Clase Individual_v4 que representa un miembro de la poblaci�n
public class Individual_v4 {
    public int[] Genes { get; private set; }
    public float Fitness { get; private set; }
    private int mazeWidth;
    private int mazeHeight;

    public Individual_v4(int mazeWidth, int mazeHeight) {
        this.mazeWidth = mazeWidth;
        this.mazeHeight = mazeHeight;
        Genes = new int[mazeWidth * mazeHeight];

        // Inicializar los genes con valores aleatorios (0 para camino, 1 para muro)
        for (int i = 0; i < Genes.Length; i++) {
            Genes[i] = Random.Range(0, 2);
        }
    }

    public void CalculateFitness() {
        // Funci�n de aptitud simple que minimiza el n�mero de muros
        Fitness = 0;
        for (int i = 0; i < Genes.Length; i++) {
            Fitness += Genes[i];
        }
    }

    public void Mutate(float mutationRate) {
        // Aplicar mutaci�n a los genes seg�n la tasa de mutaci�n
        for (int i = 0; i < Genes.Length; i++) {
            if (Random.value < mutationRate) {
                Genes[i] = 1 - Genes[i];
            }
        }
    }
}