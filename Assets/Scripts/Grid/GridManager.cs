using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerClasses;
using System.Linq;

namespace GridGame {
	public class GridManager : MonoSingleton<GridManager> {

		GridBuilder builder;
		public Selector tileSelector;

		public List<Tile> tiles = new List<Tile>();
		public List<GridOccupier> occupiers = new List<GridOccupier>();
		public Tile selectedTile;
		public GridOccupier selectedOccupier;

		protected override void Awake() {
			base.Awake();
			builder = new StandardGridBuilder(this);
			builder.GenerateMap();
			builder.SpawnBuildings();
			builder.SpawnUnits();
			tileSelector = gameObject.AddComponent(typeof(Selector)) as Selector;
		}

		public void Update() {
			/* 
			TODO: 
				Completely refactor this code, it hurts my very soul.  If statements are verbose due to
				console logs and if we had a GridOccupier as part of a tile, we wouldn't need the OccupierFromClick
				code or the logic to handle selectedOccupier effectively.
			*/
			bool tileChange = false;

			// Check if a tile was selected and set selected tile if it was
			Tile tempSelectedTile = tileSelector.GetTileFromClick();
			if (tempSelectedTile != null) {
				Debug.Log("Tile clicked!");
				this.selectedTile = tempSelectedTile;
				tileChange = true;
			}
			// Check if an occupier was selected and set selected tile if it was
			GridOccupier tempSelectedOccupier = tileSelector.GetOccupierFromClick();
			if (tempSelectedOccupier != null) {
				Debug.Log("Occupier selected!");
				this.selectedOccupier = tempSelectedOccupier;
			}
			else if (tempSelectedOccupier == null && tileChange == true){
				// Clear currently selected occupier if we selected an empty tile
				Debug.Log("No occupier currently selected.");
				this.selectedOccupier = null;
			}

			// GridOccupier is no longer selected if it is moved from the currently selected tile
			if(selectedOccupier != null && selectedOccupier.coordinate != selectedTile.coordinate)
				selectedOccupier = null;
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
				if(tileObject.transform.position == CoordinateToWorldPos(tile.coordinate) && tileObject.tag == "Tile") {
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

		public GridOccupier GetOccupierFromCoord(Coordinate occupierLoc) {
			/*
				Iterate through all occupiers in the grid, if occupier matches coordinates return it,
				otherwise return null.
			*/
			for (int i = 0; i < occupiers.Count; i++) {
				if (occupiers[i].coordinate.x == occupierLoc.x && occupiers[i].coordinate.y == occupierLoc.y)
					return occupiers[i];
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
				if(_occupier.transform.position == CoordinateToWorldPos(occupier.coordinate) && _occupier.tag == "GridObject") {
					Destroy(_occupier.gameObject);
				}
			}
		}

		public void MoveOccupier(GridOccupier occupier, Coordinate toCoordinate) {
			foreach(Transform _occupier in transform) {
				if(_occupier.transform.position == CoordinateToWorldPos(occupier.coordinate) && _occupier.tag == "GridObject") {
					_occupier.transform.position = CoordinateToWorldPos(toCoordinate);
					occupier.coordinate = toCoordinate;
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

		public Stack<Coordinate> FindWalkablePath(Coordinate fromCoordinate, Coordinate toCoordinate) {
			//Get a list of all the eligible coordinates. This will be used for:
			//	Reducing the amount of coordinates we need to consider.
			//	Evaluating neighbouring coords.
			//Coordinate will be eligible if:
			//	It is walkable
			//	There is no occupier on that coordinate (except for on the starting tile).
			List<Coordinate> walkableCoords = tiles
																				.FindAll(tile => tile.walkable && !occupiers.Any(occ => occ.coordinate == tile.coordinate && occ.coordinate != fromCoordinate))
																				.ConvertAll(tile => tile.coordinate);
			Debug.Log(walkableCoords.Count);
			if(!walkableCoords.Contains(fromCoordinate) || !walkableCoords.Contains(toCoordinate)) return null;
			//Set of coordinates we've already evaluated.
			List<Coordinate> closedSet = new List<Coordinate>();
			// Set of coordinates to still to be considered.
			List<Coordinate> openSet = new List<Coordinate>();
			//For each tile (key), this will contain the neighbour (value) with the closest path from the start point.
			Dictionary<Coordinate, Coordinate> nearestNeighbour = new Dictionary<Coordinate, Coordinate>();
			//For each coordinate, get the actual cost of getting from the starting tile to that tile.
			Dictionary<Coordinate, int> costFromStart = new Dictionary<Coordinate, int>();
			//For each coordinate, get the expected cost for a path going from start to finish through this tile.
			Dictionary<Coordinate, int> totalExpectedCost = new Dictionary<Coordinate, int>();
			//For each coordinate, store the heuristic cost to the end of the route.

			//Initialise stuff
			openSet.Add(fromCoordinate);
			foreach(Coordinate coord in walkableCoords) {
				totalExpectedCost[coord] = int.MaxValue;
			}
			costFromStart[fromCoordinate] = 0;
			totalExpectedCost[fromCoordinate] = fromCoordinate.ManhattanDistance(toCoordinate);

			while(openSet.Count > 0) {
				Coordinate considered = openSet.OrderByDescending(coordinate => totalExpectedCost[coordinate]).First();
				if(considered == toCoordinate) {
					//We're now considering the target coord.
					//Therefore we've successfully found a path.
					//Build a queue from origin to target.
					Stack<Coordinate> path = new Stack<Coordinate>();
					path.Push(toCoordinate);

					Coordinate current = toCoordinate;
					while(!path.Contains(fromCoordinate)) {
						path.Push(nearestNeighbour[current]);
						current = nearestNeighbour[current];
					}
					return path;
				}

				List<Coordinate> neighbours = walkableCoords.FindAll(coord => considered.ManhattanDistance(coord) == 1);
				openSet.Remove(considered);
				closedSet.Add(considered);
				foreach(Coordinate neighbour in neighbours) {
					// Ignore if we've already considered this neighbour.
					if(closedSet.Contains(neighbour)) continue;

					int costFromStartToNeighbour = costFromStart[considered] + considered.ManhattanDistance(neighbour);

					if(!openSet.Contains(neighbour)) {
						// Consider the neighbour in a future loop
						openSet.Add(neighbour);
					}
					else if (costFromStartToNeighbour >= costFromStart[neighbour]) {
						//Neighbour must be in the closed set here.
						//We've already found at worst an equidistant path from start to neighbour.
						continue;
					}

					//We've now found the best route from start to neighbour.
					nearestNeighbour[neighbour] = considered;
					costFromStart[neighbour] = costFromStartToNeighbour;
					totalExpectedCost[neighbour] = costFromStartToNeighbour + neighbour.ManhattanDistance(toCoordinate);
				}
			}
			//We've exhausted all possible travesal paths,
			//However still didn't get to our target. Thus
			//no walkable path exists.
			return null;
		}
	}
}
