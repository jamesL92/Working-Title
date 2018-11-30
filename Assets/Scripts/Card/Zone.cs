using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public abstract class Zone: MonoBehaviour, IPointerClickHandler {

  public delegate void ClickHandler();
  public event ClickHandler OnClick;
  [SerializeField] protected bool CardsVisible;
  protected List<Card> Cards = new List<Card>();

  public int Capacity;

  public virtual bool AddCard(Card card) {
    if(Cards.Count >= Capacity) {
      return false;
    }
    Cards.Add(card);
    card.transform.SetParent(transform);
    card.Visible = CardsVisible;
    card.CurrentZone = this;
    return true;
  }

  public virtual bool RemoveCard(Card card) {
    if(Cards.Count <= 0) {
      return false;
    }
    Cards.Remove(card);
    card.transform.SetParent(card.GetComponentInParent<Canvas>().transform);
    card.Visible = CardsVisible;
    if(card.CurrentZone == this) {
      card.CurrentZone = null;
    }
    return true;
  }
  void IPointerClickHandler.OnPointerClick(PointerEventData data) {
    if(OnClick != null) {
      OnClick();
    }
  }

  public void DestroyUnit() {

  }
}
