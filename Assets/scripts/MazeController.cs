using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeController : MonoBehaviour {
    // Mapa del laberinto
    public int[,] map;
    // Posición de inicio y fin en el laberinto
    public Vector2 startPosition;
    public Vector2 endPosition;
    // Prefabs para representar los muros, inicio, fin y camino en el laberinto
    public GameObject wallPrefab;
    public GameObject exitPrefab;
    public GameObject startPrefab;
    public GameObject pathPrefab;
    // Instancia del algoritmo genético
    public GeneticAlgorithm geneticAlgorithm;
    // Lista de direcciones del mejor individuo
    public List<int> fittestDirections;
    // Lista de tiles del camino
    public List<GameObject> pathTiles;
    // Objeto para mostrar información en la pantalla
    public GameObject text;

    // Retorna el prefab correspondiente según el tipo de tile
    public GameObject PrefabByTile(int tile) {
		if (tile == 1) return wallPrefab;
		if (tile == 5) return startPrefab;
		if (tile == 8) return exitPrefab;
		return null;
	}

    // Mueve la posición en la dirección indicada
    public Vector2 Move(Vector2 position, int direction) {
		switch (direction) {
		case 0: // North
			if (position.y - 1 < 0 || map [(int)(position.y - 1), (int)position.x] == 1) {
				break;
			} else {
				position.y -= 1;
			}
			break;
		case 1: // South
			if (position.y + 1 >= map.GetLength (0) || map [(int)(position.y + 1), (int)position.x] == 1) {
				break;
			} else {
				position.y += 1;
			}
			break;
		case 2: // East
			if (position.x + 1 >= map.GetLength (1) || map [(int)position.y, (int)(position.x + 1)] == 1) {
				break;
			} else {
				position.x += 1;
			}
			break;
		case 3: // West
			if (position.x - 1 < 0 || map [(int)position.y, (int)(position.x - 1)] == 1) {
				break;
			} else {
				position.x -= 1;
			}
			break;
		}
		return position;
	}

    // Calcula la aptitud de una ruta dada una lista de direcciones
    public double TestRoute(List<int> directions) {
		Vector2 position = startPosition;

		for (int directionIndex = 0; directionIndex < directions.Count; directionIndex++) {
			int nextDirection = directions [directionIndex];
			position = Move (position, nextDirection);
		}

		Vector2 deltaPosition = new Vector2(
			Math.Abs(position.x - endPosition.x),
			Math.Abs(position.y - endPosition.y));
		double result = 1 / (double)(deltaPosition.x + deltaPosition.y + 1);
		if (result == 1)
			Debug.Log ("TestRoute result=" + result + ",("+position.x+","+position.y+")");
		return result;
	}

    // Población inicial del laberinto con los prefabs
    public void Populate() {
		Debug.Log ("length(0)=" + map.GetLength(0));
		Debug.Log ("length(1)=" + map.GetLength(1));

		for (int y = 0; y < map.GetLength(0); y++) {
			for (int x = 0; x < map.GetLength(1); x++) {
				GameObject prefab = PrefabByTile (map [y, x]);
				if (prefab != null) {
					GameObject wall = Instantiate (prefab);
					wall.transform.position = new Vector3 (x, 0, -y);
				}
			}
		}
	}

    // Inicialización del laberinto y el algoritmo genético
    void Start () {
        // Mapa del laberinto (0: espacio vacío, 1: muro, 5: inicio, 8: salida)
        map = new int[,] {
			{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
			{1,0,1,0,0,0,0,0,1,1,1,0,0,0,5},
			{1,0,0,0,0,0,0,0,1,1,1,0,0,0,1},
			{1,0,0,0,1,1,1,0,0,1,0,0,0,0,1},
			{1,0,0,0,1,1,1,0,0,0,0,0,1,0,1},
			{1,1,0,0,1,1,1,0,0,0,0,0,1,0,1},
			{8,0,0,0,0,1,0,0,0,0,1,1,1,0,1},
			{1,0,1,1,0,0,0,1,0,0,0,0,0,0,1},
			{1,0,1,1,0,0,0,1,0,0,0,0,0,0,1},
			{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
		};
        // Población del mapa inicial
        Populate();
        // Posición de inicio y fin
        startPosition = new Vector2(14f, 1f);
        endPosition = new Vector2(0f, 6f);
        fittestDirections = new List<int>();
        pathTiles = new List<GameObject>();

        // Inicialización del algoritmo genético
        geneticAlgorithm = new GeneticAlgorithm ();
		geneticAlgorithm.mazeController = this;
		geneticAlgorithm.Run ();
	}

    // Borra los tiles del camino actual
    public void ClearPathTiles() {
		foreach (GameObject pathTile in pathTiles) {
			Destroy(pathTile);
		}
		pathTiles.Clear();
	}

    // Renderiza el camino del cromosoma más apto
    public void RenderFittestChromosomePath() {
		ClearPathTiles ();
		Genome fittestGenome = geneticAlgorithm.genomes[geneticAlgorithm.fittestGenome];
		List<int> fittestDirections = geneticAlgorithm.Decode (fittestGenome.bits);
		Vector2 position = startPosition;

		foreach (int direction in fittestDirections) {
			position = Move (position, direction);
			GameObject pathTile = Instantiate (pathPrefab);
			pathTile.transform.position = new Vector3(position.x, 0, -position.y);
			pathTiles.Add (pathTile);
		}
	}

    // Actualización en cada frame
    void Update () {
		if (geneticAlgorithm.busy) geneticAlgorithm.Epoch ();
		RenderFittestChromosomePath ();

        // Mostrar la generación actual y la posición final alcanzada
        TextMesh textMesh = text.GetComponent<TextMesh> ();
		Vector3 lastPosition = pathTiles.Last ().transform.position;
		textMesh.text = "Generation: " + geneticAlgorithm.generation + " (" + lastPosition.x + "," + lastPosition.z + ")";
	}
}
