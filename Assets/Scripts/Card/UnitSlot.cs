using UnityEngine;

public class UnitSlot: Zone {
  public override bool AddCard(Card card) {
    card.Playable = false;
    return base.AddCard(card);
  }
}