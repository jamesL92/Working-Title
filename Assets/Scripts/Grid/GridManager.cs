using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridGame {
	public class GridManager : MonoBehaviour {

		//Dimensions of the grid
		public int gridWidth = 20;
		public int gridHeight = 20;

		// How big should the initial grid be.
		[SerializeField] int initialWalkableSquareSize = 2;

		//Prefab References
		[SerializeField] GameObject walkableTilePrefab;
		[SerializeField] GameObject unwalkableTilePrefab;

		void Start() {
			GenerateMap();
		}

		void GenerateMap() {
			for(int x=0; x<gridWidth; x++) {
				for(int y=0; y<gridHeight; y++) {

					GameObject tileToSpawn;
					//Choose tile to instantiate.
					if(
						(x < initialWalkableSquareSize && y < initialWalkableSquareSize) ||
						(x >= gridWidth - initialWalkableSquareSize && y >= gridHeight - initialWalkableSquareSize)
					) {
						tileToSpawn = walkableTilePrefab;
					} else
						tileToSpawn = unwalkableTilePrefab;

					// Spawn the tile
					GameObject spawnedTile = Instantiate(tileToSpawn, new Vector3( x, y, 0f ), Quaternion.identity) as GameObject;
					spawnedTile.transform.parent = transform;
					spawnedTile.name = tileToSpawn.name; // Get rid of the annoying (clone) at the end of instantiated object's name
				}
			}
		}
	}
}