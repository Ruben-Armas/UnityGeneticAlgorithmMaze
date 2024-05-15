using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genome {
	public List<int> bits;
	public double fitness;

    // Constructor para inicializar un genoma vacío
    public Genome() {
		Initialize ();
	}

    // Constructor para inicializar un genoma con una longitud específica de bits
    public Genome(int numBits) {
		Initialize ();

        // Crear bits aleatorios para el genoma
        for (int i = 0; i < numBits; i++) {
			System.Random randomNumberGen = new System.Random(DateTime.Now.GetHashCode() * SystemInfo.processorFrequency.GetHashCode());

		    bits.Add(randomNumberGen.Next(0, 2));
		}
	}

    // Inicializar el genoma
    private void Initialize() {
		fitness = 0;
		bits = new List<int> ();
	}
}
