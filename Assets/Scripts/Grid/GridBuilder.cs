using System;

namespace GridGame {
  // Base class for grid builders.
  [Serializable]
  public abstract class GridBuilder {
    protected GridManager grid;

    public GridBuilder(GridManager grid) {
      this.grid = grid;
    }
    public abstract void GenerateMap();
    public abstract void SpawnBuildings();
    public abstract void SpawnUnits();
  }
}
