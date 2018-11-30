using UnityEngine;

public class UnitSlot: Zone {

  public UnitSlot oppositeSlot;

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
        Debug.Log(string.Format("Card is {0} is attacking card in slot {1}.", name, oppositeSlot.name));
      }
  }
}