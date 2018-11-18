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
    public Coordinate spawningCoordinate;

    public Building(Coordinate coordinate, Player owner): base(coordinate, owner) {
      prefab = PrefabManager.instance.castlePrefab;
    }

    public bool SpawnUnit() {
      foreach(GridOccupier occupier in GridManager.instance.occupiers) {
        if(occupier.coordinate == spawningCoordinate) return false;
      }
      GridManager.instance.AddOccupier(new Unit(spawningCoordinate, owner));
      return true;
    }
  }
}