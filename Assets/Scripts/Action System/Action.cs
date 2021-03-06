using System.Collections;
using PlayerClasses;
using GridGame;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Working_Title.Assets.Scripts;

namespace ActionSystem {
  public abstract class Action {

    protected Player player;

    public Action(Player player) {
      this.player = player;
    }
    public abstract IEnumerator Perform();
    public abstract void Undo();
  }

  public class GainGoldAction: Action {

    private bool performed;

    public GainGoldAction(Player player): base(player) {}

    public override IEnumerator Perform() {
      if(!performed){
        player.gold++;
        performed = true;
        yield return null;
      }
    }

    public override void Undo() {
      if(performed) {
        player.gold--;
        performed = false;
      }
    }
  }

  public class BuildTilesAction: Action {

    private bool performed;
    private int costPerTile = 2;
    private List<Coordinate> coordinatesWithTilesBuilt = new List<Coordinate>();

    public BuildTilesAction(Player player): base(player) {}

    public override IEnumerator Perform() {
      if(!performed && player.gold >= costPerTile){
        //Get list of tiles the player can start their build next to.
        List<Coordinate> coordsThePlayerControls = GridManager.instance.FloodFillForPlayer(player);
        //On mouse down, check if the player has selected a starting point.
        Coordinate startingCoord;


        while(true) {
          //Wait for player to click on a tile.
          //Tile should be adjacent to one the player controls.
          //Tile should not be walkable (as we're building walkable tiles)
          if(Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
              Coordinate clickedCoordinate = GridManager.instance.WorldPosToCoordinate(hit.point);
              if(
                coordsThePlayerControls.FindIndex(coord => coord.ManhattanDistance(clickedCoordinate) == 1) > -1
                && GridManager.instance.tiles.Find(tile => !tile.walkable && tile.coordinate == clickedCoordinate) != null
              ) {
                startingCoord = clickedCoordinate;
                break;
              }
            }
          }
          //Wait until the next frame if user hasn't selected anything.
          yield return null;
        }


        //On mouse drag, update the list of tiles to build.
        Coordinate hoveredCoordinate = startingCoord;
        while(true) {
          RaycastHit hit;
          if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
            hoveredCoordinate = GridManager.instance.WorldPosToCoordinate(hit.point);
            //Figure out if path should be draw vertical or horizontal.
            int xDiff = Mathf.Abs(hoveredCoordinate.x - startingCoord.x);
            int yDiff = Mathf.Abs(hoveredCoordinate.y - startingCoord.y);
            if(xDiff > yDiff) hoveredCoordinate.y = startingCoord.y;
            else hoveredCoordinate.x = startingCoord.x;
          }
          if(Input.GetMouseButtonUp(0)) {
            break;
          }
          yield return null;
        }

        //On mouse up, build the tiles.
        Coordinate nextCoord = startingCoord;
        while(true) {
          Tile tileAtCoordinate = GridManager.instance.tiles.Find(tile => tile.coordinate == nextCoord);
          if( tileAtCoordinate != null && !tileAtCoordinate.walkable) {
            GridManager.instance.AddTile(new WalkableTile(nextCoord));
            player.gold -= costPerTile;
            coordinatesWithTilesBuilt.Add(nextCoord);
            if(player.gold < costPerTile) {
              //Haven't got gold to keep building, stop building.
              break;
            }
          }
          if(nextCoord == hoveredCoordinate || startingCoord == hoveredCoordinate) {
            break;
          }
          nextCoord = new Coordinate(
            nextCoord.x + (nextCoord.x < hoveredCoordinate.x ? 1 : nextCoord.x == hoveredCoordinate.x ? 0 : -1),
            nextCoord.y + (nextCoord.y < hoveredCoordinate.y ? 1 : nextCoord.y == hoveredCoordinate.y ? 0 : -1)
          );
        }
        performed = true;
        yield return null;
      }
    }

    public override void Undo() {
      if(performed) {
        foreach(Coordinate coord in coordinatesWithTilesBuilt) {
          GridManager.instance.AddTile(new UnwalkableTile(coord));
          player.gold += costPerTile;
        }
        performed = false;
      }
    }
  }

  public class SpawnUnitAction: Action {

    private int spawnUnitCost = 5;
    private bool performed;
    private Unit spawnedUnit;
    public SpawnUnitAction(Player player): base(player) {}

    public override IEnumerator Perform() {
      if(player.gold < spawnUnitCost || performed) yield break;

      Building building = (Building)GridManager.instance.occupiers
                            .Find(occ => occ.owner == player && occ.GetType() == typeof(Building));
      spawnedUnit = building.SpawnUnit();
      if(spawnedUnit != null) {
        player.gold -= spawnUnitCost;
        performed = true;
      }
    }

    public override void Undo() {
      if(performed) {
        GridManager.instance.RemoveOccupier(spawnedUnit);
        player.gold += spawnUnitCost;
        performed = false;
      }
    }
  }

  public class MoveUnitAction: Action {

    private bool performed;
    private GridOccupier selectedUnit = null;
    private Coordinate fromCoord;
    private Coordinate toCoord;
    private int maxMovementSpeed = 5;
    public MoveUnitAction(Player player): base(player) {}

    public override IEnumerator Perform() {
      if(!performed) {
        while(true) {
          if(Input.GetMouseButtonDown(0)) {
            RaycastHit hit;

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
              Coordinate clickedCoordinate = GridManager.instance.WorldPosToCoordinate(hit.point);
              GridOccupier _selectedUnit = GridManager.instance.occupiers
                            .Find(occ => occ.GetType() == typeof(Unit) && occ.coordinate == clickedCoordinate && occ.owner == player);
              if(_selectedUnit != null) {
                // We've picked a new unit.
                selectedUnit = _selectedUnit;
              }
              else if(GridManager.instance.occupiers.Any(occ => occ.coordinate == clickedCoordinate)) {
                // We've picked an invalid unit.
              }
              else if(selectedUnit != null && GridManager.instance.tiles.Find(tile => tile.coordinate == clickedCoordinate).walkable) {
                // We have a unit selected, and we've clicked on a walkable tile.
                // Check if we can build a path from our selected unit to our target coordinate.
                Stack<Coordinate> path = GridManager.instance.FindWalkablePath(selectedUnit.coordinate, clickedCoordinate);
                if(path != null && path.Count < maxMovementSpeed) {
                  fromCoord = selectedUnit.coordinate;
                  toCoord = clickedCoordinate;
                  GridManager.instance.MoveOccupier(selectedUnit, toCoord);
        performed = true;
        yield break;
      }
    }
            }
          }
          yield return null;
        }
      }
    }

    public override void Undo() {
      if(performed) {
        GridManager.instance.MoveOccupier(selectedUnit, fromCoord);
        selectedUnit = null;
        performed = false;
      }
    }
  }
}