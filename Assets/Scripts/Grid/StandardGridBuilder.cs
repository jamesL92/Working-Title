using UnityEngine;
using System;

namespace GridGame {
  [Serializable]
  public class StandardGridBuilder: GridBuilder {

    //Dimensions of the grid
    public int gridWidth = 20;
    public int gridHeight = 20;

    // How big should the initial grid be.
    [SerializeField] int initialWalkableSquareSize = 2;

    public StandardGridBuilder(GridManager grid): base(grid) {}

    public override void GenerateMap() {
      for(int x=0; x<gridWidth; x++) {
        for(int y=0; y<gridHeight; y++) {

          GameObject tileToSpawn;
          //Choose tile to instantiate.
          if(
            (x < initialWalkableSquareSize && y < initialWalkableSquareSize) ||
            (x >= gridWidth - initialWalkableSquareSize && y >= gridHeight - initialWalkableSquareSize)
          ) {
            tileToSpawn = grid.walkableTilePrefab;
          } else
            tileToSpawn = grid.unwalkableTilePrefab;

          // Spawn the tile
          GameObject spawnedTile = GameObject.Instantiate(tileToSpawn, new Vector3( x, y, 0f ), Quaternion.identity) as GameObject;
          spawnedTile.transform.parent = grid.transform;
          spawnedTile.name = tileToSpawn.name; // Get rid of the annoying (clone) at the end of instantiated object's name
        }
      }
    }

    public override void SpawnBuildings() {
      //Spawn human player's building.
      GameObject building = GameObject.Instantiate(grid.castlePrefab, new Vector3(0, 0, -1f), Quaternion.identity);
      building.transform.parent = grid.transform;
      building.name = grid.castlePrefab.name;
      grid.buildings.Add(building);
      //Spawn opponent's building.
      building = GameObject.Instantiate(grid.castlePrefab, new Vector3(gridWidth-1, gridHeight-1, -1f), Quaternion.identity);
      building.transform.parent = grid.transform;
      building.name = grid.castlePrefab.name; // Get rid of the annoying (clone) at the end of instantiated object's name
      grid.buildings.Add(building);
    }
  }
}
