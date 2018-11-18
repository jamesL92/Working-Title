using System.Collections;
using PlayerClasses;
using UnityEngine;
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

    public BuildTilesAction(Player player): base(player) {}

    public override IEnumerator Perform() {
      if(!performed){
        Debug.Log("Building Tiles");
        performed = true;
        yield return null;
      }
    }

    public override void Undo() {
      if(performed) {
        Debug.Log("Undoing Built Tiles");
        performed = false;
      }
    }
  }
}