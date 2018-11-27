using PlayerClasses;

namespace ActionSystem {
  public class ActionFactory {
    private ActionManager manager;

    public ActionFactory(ActionManager manager) {
      this.manager = manager;
    }

    public Action CreateAction(ActionType actionType, Player player) {
      switch(actionType) {
        case ActionType.GAIN_GOLD:
          return new GainGoldAction(player);
        case ActionType.BUILD_TILES:
          return new BuildTilesAction(player);
        case ActionType.SPAWN_UNIT:
          return new SpawnUnitAction(player);
        case ActionType.MOVE_UNIT:
          return new MoveUnitAction(player);
        case ActionType.ATTACK:
          return new AttackAction(player);
        default:
          return null;
      }
    }
  }
}