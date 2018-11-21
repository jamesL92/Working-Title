using UnityEngine;

namespace GridGame {
  public abstract class Tile {
    public bool walkable;
    public Coordinate coordinate;
    public GameObject tilePrefab;

    public Tile(Coordinate coordinate) {
      this.coordinate = coordinate;
    }
  }

  public class WalkableTile: Tile {
    public WalkableTile(Coordinate coordinate): base(coordinate) {
      this.walkable = true;
      this.tilePrefab = PrefabManager.instance.walkableTilePrefab;
      // Add prefab to the tile layer
      tilePrefab.layer = LayerMask.NameToLayer("Tile");
    }
  }
  public class UnwalkableTile: Tile {
    public UnwalkableTile(Coordinate coordinate): base(coordinate) {
      this.walkable = false;
      this.tilePrefab = PrefabManager.instance.unwalkableTilePrefab;
      // Add prefab to the tile layer
      tilePrefab.layer = LayerMask.NameToLayer("Tile");
    }
  }
}