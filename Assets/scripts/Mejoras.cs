using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mejoras : MonoBehaviour {
    /*
    #region 1 y 2 -- Ajuste del Tamaño de la Población y Tasas de Mutación y Cruce

    public int populationSize = 200;    // Incrementado para mayor diversidad
    public double crossoverRate = 0.8;  // Ajustado para mayor exploración
    public double mutationRate = 0.005; // Tasa de mutación adaptativa

    // Implementación de tasa de mutación adaptativa
    public void AdjustMutationRate() {
        double diversity = CalculateDiversity();
        mutationRate = 0.005 + (0.02 * (1 - diversity));
    }
    #endregion

    #region 3.1 -- Selección por Torneo
    public Genome TournamentSelection() {
        int tournamentSize = 5;
        List<Genome> tournament = new List<Genome>();

        for (int i = 0; i < tournamentSize; i++) {
            int randomIndex = UnityEngine.Random.Range(0, populationSize);
            tournament.Add(genomes[randomIndex]);
        }

        return tournament.OrderByDescending(g => g.fitness).First();
    }
    #endregion

    #region 3.2 -- Implementación de Elitismo
    public void Elitism() {
        List<Genome> newPopulation = new List<Genome>();

        // Preservar los mejores individuos
        int eliteCount = (int)(populationSize * 0.1);
        newPopulation.AddRange(genomes.OrderByDescending(g => g.fitness).Take(eliteCount));

        // Crear nuevos individuos para completar la población
        while (newPopulation.Count < populationSize) {
            Genome mom = TournamentSelection();
            Genome dad = TournamentSelection();
            Genome baby1 = new Genome();
            Genome baby2 = new Genome();
            Crossover(mom.bits, dad.bits, baby1.bits, baby2.bits);
            Mutate(baby1.bits);
            Mutate(baby2.bits);
            newPopulation.Add(baby1);
            newPopulation.Add(baby2);
        }

        genomes = newPopulation;
    }
    #endregion


    #region 4.1 -- Fitness Sharing
    public void FitnessSharing() {
        double sharingThreshold = 0.5; // Umbral para considerar individuos similares
        double alpha = 1.0; // Exponente de penalización

        foreach (Genome genome in genomes) {
            double sharingSum = 0.0;
            foreach (Genome otherGenome in genomes) {
                if (genome != otherGenome) {
                    double distance = HammingDistance(genome.bits, otherGenome.bits);
                    if (distance < sharingThreshold) {
                        sharingSum += 1.0 - Math.Pow(distance / sharingThreshold, alpha);
                    }
                }
            }
            genome.fitness /= (1.0 + sharingSum); // Ajustar la aptitud basada en la densidad local
        }
    }

    public double HammingDistance(List<int> bits1, List<int> bits2) {
        int distance = 0;
        for (int i = 0; i < bits1.Count; i++) {
            if (bits1[i] != bits2[i]) {
                distance++;
            }
        }
        return distance;
    }
    #endregion

    #region 4.2 -- Niche Penalty
    public void ApplyNichePenalties() {
        int nicheSize = 5; // Tamaño del nicho
        double nichePenalty = 0.5; // Penalización aplicada a nichos superpoblados

        Dictionary<int, int> nicheCounts = new Dictionary<int, int>();

        // Contar la cantidad de individuos en cada nicho
        foreach (Genome genome in genomes) {
            int nicheID = CalculateNicheID(genome.bits);
            if (!nicheCounts.ContainsKey(nicheID)) {
                nicheCounts[nicheID] = 0;
            }
            nicheCounts[nicheID]++;
        }

        // Aplicar penalizaciones
        foreach (Genome genome in genomes) {
            int nicheID = CalculateNicheID(genome.bits);
            if (nicheCounts[nicheID] > nicheSize) {
                genome.fitness *= nichePenalty;
            }
        }
    }

    public int CalculateNicheID(List<int> bits) {
        // Implementar una función para calcular el ID de nicho basado en el genoma
        // Puede ser un hash, una suma de bits, etc.
        return bits.Sum();
    }
    #endregion

    #region Implementación de los apartados 4.
    public void UpdateFitnessScores() {
        fittestGenome = 0;
        bestFitnessScore = 0;
        totalFitnessScore = 0;

        for (int i = 0; i < populationSize; i++) {
            List<int> directions = Decode(genomes[i].bits);
            genomes[i].fitness = mazeController.TestRoute(directions);
            totalFitnessScore += genomes[i].fitness;

            if (genomes[i].fitness > bestFitnessScore) {
                bestFitnessScore = genomes[i].fitness;
                fittestGenome = i;

                // Has chromosome found the exit?
                if (genomes[i].fitness == 1) {
                    busy = false; // stop the run
                    return;
                }
            }
        }

        // Aplicar estrategias de diversificación
        FitnessSharing();
        ApplyNichePenalties();

        // Recalcular la aptitud total después de aplicar penalizaciones
        totalFitnessScore = genomes.Sum(genome => genome.fitness);
    }
    #endregion
    */
}
