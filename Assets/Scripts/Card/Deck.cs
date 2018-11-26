using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Deck: Zone {

  public bool IsEmpty {
    get { return Cards.Count <= 0; }
  }

  public void DrawCard(Zone to) {
    if(!IsEmpty && to != null) {
      Card card = Cards[0];
      if(to.AddCard(card)) {
        Cards.Remove(card);
        return;
      }
    }
    //TODO: Improve this error - can be thrown if `to` is full.
    throw new KeyNotFoundException("Cannot draw from deck as it is empty.");
  }

  public void Shuffle() {
    for(int i=0; i<Cards.Count; i++) {
      int j = Random.Range(i, Cards.Count);
      Card temp = Cards[j];
      Cards[j] = Cards[i];
      Cards[i] = temp;
      Cards[i].transform.SetSiblingIndex(i);
    }
  }
}