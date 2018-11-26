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
    return true;
  }

  void IPointerClickHandler.OnPointerClick(PointerEventData data) {
    Debug.Log("I was clicked!");
    if(OnClick != null) {
      OnClick();
    }
  }
}
