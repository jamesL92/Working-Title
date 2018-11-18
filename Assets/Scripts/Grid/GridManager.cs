using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridGame {
	public class GridManager : MonoSingleton<GridManager> {

		public List<GameObject> buildings = new List<GameObject>();
		public List<GameObject> units = new List<GameObject>();

		GridBuilder builder;

		public List<Tile> tiles = new List<Tile>();

		protected override void Awake() {
			base.Awake();
			builder = new StandardGridBuilder(this);
			builder.GenerateMap();
			builder.SpawnBuildings();
			builder.SpawnUnits();
		}

		public void AddTile(Tile tile) {
			//Add new tile to the grid, or replace existing tile.
			Tile currentTileAtCoordinate = tiles.Find(_tile => _tile.coordinate == tile.coordinate);
			if(currentTileAtCoordinate != null) {
				RemoveTile(currentTileAtCoordinate);
			}
			tiles.Add(tile);
			GameObject newTile = Instantiate(tile.tilePrefab, CoordinateToWorldPos(tile.coordinate), Quaternion.identity) as GameObject;
			newTile.name = tile.tilePrefab.name;
			newTile.transform.parent = transform;
		}

		private void RemoveTile(Tile tile) {
			if(!tiles.Contains(tile)) return;
				//Remove tile from the grid.
				tiles.Remove(tile);
				//Destroy the game object associated with that tile.
				foreach(Transform tileObject in transform) {
					if(tileObject.transform.position == CoordinateToWorldPos(tile.coordinate)) {
						Destroy(tileObject.gameObject);
					}
				}
		}
		public Vector3 CoordinateToWorldPos(Coordinate coordinate) {
			return new Vector3(coordinate.x, coordinate.y, transform.position.z);
		}
	}
}
