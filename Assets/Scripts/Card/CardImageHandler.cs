using UnityEngine;
using UnityEngine.UI;

public class CardImageHandler: MonoBehaviour {
  private Image image;
  private Card card;

  void Start() {
    image = GetComponent<Image>();
    card = GetComponent<Card>();
  }

  void LateUpdate() {
    if(card != null && image != null) {
      image.sprite = card.Visible ? PrefabManager.instance.cardFrontSprite : PrefabManager.instance.cardBackSprite;
    }
  }
}