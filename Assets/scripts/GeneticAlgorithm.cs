using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm {
	public List<Genome> genomes;
	public List<Genome> lastGenerationGenomes;

	public int populationSize = 140;
	public double crossoverRate = 0.7f;
	public double mutationRate = 0.001f;
	public int chromosomeLength = 70;
	public int geneLength = 2;

	public int fittestGenome;
	public double bestFitnessScore;
	public double totalFitnessScore;
	public int generation;
	public MazeController mazeController;
	public MazeController mazeControllerDisplay;
	public bool busy;

    // Constructor para inicializar el algoritmo genético
    public GeneticAlgorithm() {
		busy = false;
		genomes = new List<Genome> ();
		lastGenerationGenomes = new List<Genome> ();
	}

    // Mutación de los bits de un genoma
    public void Mutate(List<int> bits) {
		for (int i = 0; i < bits.Count; i++) {
            // Mutar este bit?
            if (UnityEngine.Random.value < mutationRate) {
                // Mutar el bit
                bits[i] = bits [i] == 0 ? 1 : 0;
			}
		}
	}

    // Cruce entre dos genomas
    public void Crossover(List<int> mom, List<int> dad, List<int> baby1, List<int> baby2) {
        // Si no hay cruce, simplemente copiar los padres
        if (UnityEngine.Random.value > crossoverRate || mom == dad) {
			baby1.AddRange(mom);
			baby2.AddRange(dad);

			return;
		}

		System.Random rnd = new System.Random ();
		int crossoverPoint = rnd.Next (0, chromosomeLength - 1);

        // Intercambiar bits hasta el punto de cruce
        for (int i = 0; i < crossoverPoint; i++) {
			baby1.Add (mom [i]);
			baby2.Add (dad [i]);
		}

        // Intercambiar los bits restantes después del punto de cruce
        for (int i = crossoverPoint; i < mom.Count; i++) {
			baby1.Add (dad [i]);
			baby2.Add (mom [i]);
		}
	}

    // Selección por ruleta
    public Genome RouletteWheelSelection() {
		double slice = UnityEngine.Random.value * totalFitnessScore;
		double total = 0;
		int selectedGenome = 0;

		for (int i = 0; i < populationSize; i++) {
			total += genomes [i].fitness;

			if (total > slice) {
				selectedGenome = i;
				break;
			}
		}
		return genomes[selectedGenome];
	}

    // Actualización de las puntuaciones de aptitud
    public void UpdateFitnessScores() {
		fittestGenome = 0;
		bestFitnessScore = 0;
		totalFitnessScore = 0;

		for (int i = 0; i < populationSize; i++) {
			List<int> directions = Decode (genomes [i].bits);

			genomes [i].fitness = mazeController.TestRoute (directions);

			totalFitnessScore += genomes [i].fitness;

			if (genomes [i].fitness > bestFitnessScore) {
				bestFitnessScore = genomes [i].fitness;
				fittestGenome = i;

                // Verificar si el cromosoma encontró la salida
                if (genomes [i].fitness == 1) {
					busy = false; // detener la ejecución
                    return;
				}
			}
		}
	}

    //---------------------------Decode-------------------------------------
    //
    //	Decodifica una lista de bits en una lista de direcciones (ints)
    //
    //	0=North, 1=South, 2=East, 3=West
    //-----------------------------------------------------------------------
    public List<int> Decode(List<int> bits) {
		List<int> directions = new List<int> ();

		for (int geneIndex = 0; geneIndex < bits.Count; geneIndex += geneLength) {
			List<int> gene = new List<int> ();

			for (int bitIndex = 0; bitIndex < geneLength; bitIndex++) {
				gene.Add (bits [geneIndex + bitIndex]);
			}

			directions.Add (GeneToInt (gene));
		}
		return directions;
	}

    //-------------------------------GeneToInt-------------------------------
    //	Convierte una lista de bits en un entero
    //----------------------------------------------------------------------
    public int GeneToInt(List<int> gene) {
		int value = 0;
		int multiplier = 1;

		for (int i = gene.Count; i > 0; i--) {
			value += gene [i - 1] * multiplier;
			multiplier *= 2;
		}
		return value;
	}

    // Crear la población inicial
    public void CreateStartPopulation() {
		genomes.Clear ();

		for (int i = 0; i < populationSize; i++) {
			Genome baby = new Genome (chromosomeLength);
			genomes.Add (baby);
		}
	}

    // Iniciar la ejecución del algoritmo genético
    public void Run() {
		CreateStartPopulation ();
		busy = true;
	}

    // Ejecutar una época del algoritmo genético
    public void Epoch() {
		if (!busy) return;
		UpdateFitnessScores ();

		if (!busy) {
			lastGenerationGenomes.Clear();
			lastGenerationGenomes.AddRange (genomes);
			return;
		}
		
		int numberOfNewBabies = 0;

		List<Genome> babies = new List<Genome> ();
		while (numberOfNewBabies < populationSize) {
            // Seleccionar dos padres
            Genome mom = RouletteWheelSelection ();
			Genome dad = RouletteWheelSelection ();
			Genome baby1 = new Genome();
			Genome baby2 = new Genome();
			Crossover (mom.bits, dad.bits, baby1.bits, baby2.bits);
			Mutate (baby1.bits);
			Mutate (baby2.bits);
			babies.Add (baby1);
			babies.Add (baby2);

			numberOfNewBabies += 2;
		}

        // Guardar la última generación para propósitos de visualización
        lastGenerationGenomes.Clear();
		lastGenerationGenomes.AddRange (genomes);
        // Sobrescribir la población con los nuevos bebés
        genomes = babies;

        // Incrementar el contador de generaciones
        generation++;
	}
}
