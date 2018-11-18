﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridGame {
	public class GridManager : MonoSingleton<GridManager> {

		GridBuilder builder;

		public List<Tile> tiles = new List<Tile>();
		public List<GridOccupier> occupiers = new List<GridOccupier>();

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

		public void AddOccupier(GridOccupier occupier) {
			//Add an occupier to the grid.
			//Space must be empty for it to be occupied.
			//Occupier must be placed on top of a tile.
			if(tiles.Find(tile => tile.coordinate == occupier.coordinate) == null) return;
			if(occupiers.Find(_occupier => occupier.coordinate == _occupier.coordinate) != null) return;

			occupiers.Add(occupier);
			GameObject newOccupier = Instantiate(occupier.prefab, CoordinateToWorldPos(occupier.coordinate), Quaternion.identity) as GameObject;
			newOccupier.transform.parent = transform;
			newOccupier.name = occupier.prefab.name;
		}

		public void RemoveOccupier(GridOccupier occupier) {
			if(!occupiers.Contains(occupier)) return;
			occupiers.Remove(occupier);
			foreach(Transform _occupier in transform) {
				if(_occupier.transform.position == CoordinateToWorldPos(occupier.coordinate)) {
					Destroy(_occupier.gameObject);
				}
			}
		}

		public Vector3 CoordinateToWorldPos(Coordinate coordinate) {
			return new Vector3(coordinate.x, coordinate.y, transform.position.z);
		}
	}
}
