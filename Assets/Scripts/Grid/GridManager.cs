﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerClasses;

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
				//TODO: Find a way to only search for tiles.
				if(tileObject.transform.position == CoordinateToWorldPos(tile.coordinate)) {
					Destroy(tileObject.gameObject);
				}
			}
		}

		public Tile GetTileFromCoord(Coordinate tileLoc) {
			/*
			 Iterate through all tiles in the grid, if tile matches coordinates return it,
			 otherwise return null.
			*/
			for (int i = 0; i < tiles.Count; i++) {
				if (tiles[i].coordinate.x == tileLoc.x && tiles[i].coordinate.y == tileLoc.y)
					return tiles[i];
			}
			return null;
		}

		public Tile GetTileFromClick() {
			// Layermask ignores all but the Tile layer
            int layerMask = LayerMask.GetMask("Tile");
            if(Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask)){
                    // I could use the gridmanager helper function here to retrieve coords
					Coordinate clickedCoordinate = instance.WorldPosToCoordinate(hit.point);
					return GetTileFromCoord(clickedCoordinate);
				}
            }
			return null;
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
			//TODO: Find a way to only search for other grid objects.
				if(_occupier.transform.position == CoordinateToWorldPos(occupier.coordinate)) {
					Destroy(_occupier.gameObject);
				}
			}
		}

		public Vector3 CoordinateToWorldPos(Coordinate coordinate) {
			return new Vector3(coordinate.x, coordinate.y, transform.position.z);
		}

		public Coordinate WorldPosToCoordinate(Vector3 worldPos) {
			return new Coordinate(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.y));
		}

		public List<Coordinate> FloodFillForPlayer(Player player) {
				List<Coordinate> allowedCoords = new List<Coordinate>();
				List<Coordinate> disallowedCoords = new List<Coordinate>();
				List<Coordinate> tilesToSearch = tiles.FindAll(tile => tile.walkable).ConvertAll(tile => tile.coordinate);

				//Add to lists for each current occupier.
				foreach(GridOccupier occupier in occupiers) {
					if(occupier.owner == player) allowedCoords.Add(occupier.coordinate);
					else disallowedCoords.Add(occupier.coordinate);
					tilesToSearch.Remove(occupier.coordinate);
				}

				int coordinatesAdded;
				//Iteratively check all tiles to see if they're a neighbour of a tile we've already found.
				do {
					coordinatesAdded = 0;
					Coordinate[] tilesToSearchThisLoop = tilesToSearch.ToArray();
						foreach(Coordinate coordinate in tilesToSearchThisLoop) {
							int foundCoordinateIndex = allowedCoords.FindIndex(allowed => allowed.ManhattanDistance(coordinate) == 1);
							if(foundCoordinateIndex > -1) {
							// if(allowedCoords.Find(allowed => allowed.ManhattanDistance(coordinate) == 1) != null) {
								allowedCoords.Add(coordinate);
								tilesToSearch.Remove(coordinate);
								coordinatesAdded++;
							}
						}
				} while (coordinatesAdded > 0);
				return allowedCoords;
		}
	}
}
