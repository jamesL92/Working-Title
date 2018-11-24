using UnityEngine;
using PlayerClasses;

namespace GridGame {
  public abstract class GridOccupier {
    public Coordinate coordinate;
    public GameObject prefab;
    public Player owner;
    protected int health;

    public GridOccupier(Coordinate coordinate, Player owner) {
      this.coordinate = coordinate;
      this.owner = owner;
      this.health = 100;
    }

    public void modify_health(int modifier) {
      /*
        Function to apply modifier to health (e.g. -2 or +2)
      */
      // Health cannot be greater than 100
      if (health + modifier > 100) health = 100;
      // Destroy self if health is fully depleted
      else if (health + modifier <= 0) GridManager.instance.RemoveOccupier(this);
      // Apply modifier
      else health += modifier;
    }
  }

  public class Unit: GridOccupier {
    public Unit(Coordinate coordinate, Player owner): base(coordinate, owner) {
      prefab = PrefabManager.instance.unitPrefab;
      this.health = 20;
    }




  }

  public class Building: GridOccupier {
    public Coordinate spawningCoordinate;

    public Building(Coordinate coordinate, Player owner): base(coordinate, owner) {
      prefab = PrefabManager.instance.castlePrefab;
      this.health = 50;
    }

    public Unit SpawnUnit() {
      foreach(GridOccupier occupier in GridManager.instance.occupiers) {
        if(occupier.coordinate == spawningCoordinate) return null;
      }
      Unit spawnedUnit = new Unit(spawningCoordinate, owner);
      GridManager.instance.AddOccupier(spawnedUnit);
      return spawnedUnit;
    }
  }
}