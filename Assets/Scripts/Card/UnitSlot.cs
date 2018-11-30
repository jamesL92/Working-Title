using UnityEngine;

public class UnitSlot: Zone {

  public UnitSlot oppositeSlot;
  public Commander opponentCommander;
  public Discard DiscardPile;

  private void Start() {
    OnClick += UnitSlotClickHandler;
  }

  public override bool AddCard(Card card) {
    card.Playable = false;
    return base.AddCard(card);
  }

  public Card GetUnit() {
    if(Cards.Count > 0) {
      return Cards[0];
    }
    return null;
  }

  private void UnitSlotClickHandler() {
    Card myCard = GetUnit();
    Card oppositeCard = oppositeSlot.GetUnit();
    if(myCard && oppositeCard) {
      //TODO: Refactor this when we have implemented units properly.
      oppositeSlot.DestroyCard();
    }
    else if(myCard) {
      opponentCommander.Damage(10);
    }
  }

  public void DestroyCard() {
    Card unit = GetUnit();
    if(GetUnit() != null) {
      RemoveCard(unit);
      DiscardPile.AddCard(unit);
    }
  }
}