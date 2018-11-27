namespace GridGame
{
  public struct AttackData {
    /* 
        Assumptions made:
        The only data relevant to the card game is the units involved in
        combat and the players owning each unit. All of which is contained
        within the units.

        Attacker is of type unit, which assumes only Units can initiate attacks,
        defender is a GridOccupier, which assumes things other than units ccan be attacked
        (e.g. buildings)
    */
    Unit attacker;
    GridOccupier defender;

    public AttackData(Unit attacker, GridOccupier defender) {
        this.attacker = attacker;
        this.defender = defender;
    }
  }
}