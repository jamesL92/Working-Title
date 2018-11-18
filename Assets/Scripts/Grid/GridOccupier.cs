using UnityEngine;
using PlayerClasses;

namespace GridGame {
  public abstract class GridOccupier {
    public Coordinate coordinate;
    public GameObject prefab;
    public Player owner;

    public GridOccupier(Coordinate coordinate, Player owner) {
      this.coordinate = coordinate;
      this.owner = owner;
    }
  }

  public class Unit: GridOccupier {
    public Unit(Coordinate coordinate, Player owner): base(coordinate, owner) {
      prefab = PrefabManager.instance.unitPrefab;
    }
  }

  public class Building: GridOccupier {
    public Building(Coordinate coordinate, Player owner): base(coordinate, owner) {
      prefab = PrefabManager.instance.castlePrefab;
    }
  }
}