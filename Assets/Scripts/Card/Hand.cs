public class Hand: Zone {
  public override bool AddCard(Card card) {
    card.Playable = true;
    return base.AddCard(card);
  }
}