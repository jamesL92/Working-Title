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
          Tile tileToSpawn;
          //Choose tile to instantiate.
          if(
            (x < initialWalkableSquareSize && y < initialWalkableSquareSize) ||
            (x >= gridWidth - initialWalkableSquareSize && y >= gridHeight - initialWalkableSquareSize)
          ) {
            tileToSpawn = new WalkableTile(new Coordinate(x,y));
          } else
            tileToSpawn = new UnwalkableTile(new Coordinate(x,y));

          // Add tile to the grid.
          GridManager.instance.AddTile(tileToSpawn);
        }
      }
    }

    public override void SpawnBuildings() {
      grid.AddOccupier(new Building(new Coordinate(0,0)));
      grid.AddOccupier(new Building(new Coordinate(gridWidth-1,gridHeight-1)));
    }

    public override void SpawnUnits() {
      grid.AddOccupier(new Unit(new Coordinate(1,1)));
      grid.AddOccupier(new Unit(new Coordinate(gridWidth-2,gridHeight-2)));
    }
  }
}
