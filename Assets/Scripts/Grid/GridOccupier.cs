using UnityEngine;

namespace GridGame {
  public abstract class GridOccupier {
    public Coordinate coordinate;
    public GameObject prefab;

    public GridOccupier(Coordinate coordinate) {
      this.coordinate = coordinate;
    }
  }

  public class Unit: GridOccupier {
    public Unit(Coordinate coordinate): base(coordinate) {
      prefab = PrefabManager.instance.unitPrefab;
    }
  }

  public class Building: GridOccupier {
    public Building(Coordinate coordinate): base(coordinate) {
      prefab = PrefabManager.instance.castlePrefab;
    }
  }
}