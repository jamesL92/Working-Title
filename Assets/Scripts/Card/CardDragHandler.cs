using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
  private CanvasGroup canvasGroup;
  private Card card;
  [SerializeField] private GameObject clone;
  [SerializeField] private Zone originalZone;

  void Start() {
    card = GetComponent<Card>();
    canvasGroup = GetComponent<CanvasGroup>();
  }

  public void OnBeginDrag(PointerEventData data) {
    if(card == null || !card.Playable) return;
    canvasGroup.blocksRaycasts = false;
    CreateClone();
  }

  public void OnDrag(PointerEventData data) {
    if(card == null || !card.Playable) return;
    card.transform.position = Input.mousePosition;
  }

  public void OnEndDrag(PointerEventData data) {
    if(card == null || !card.Playable) return;
    canvasGroup.blocksRaycasts = true;
    Debug.Log(data.pointerEnter.GetComponent<Zone>());
    if(data.pointerEnter != null && data.pointerEnter.GetComponent<Zone>() != null) {
      Zone droppedZone = data.pointerEnter.GetComponent<Zone>();
      if(droppedZone != null) {
        droppedZone.AddCard(card);
        originalZone = null;
        Destroy(clone);
      } else {
        ResetToClone();
      }
    }
    else {
      ResetToClone();
    }
  }

  private void CreateClone() {
    int siblingIndex = transform.GetSiblingIndex();

    clone = new GameObject();
    clone.transform.SetParent(transform);
    clone.transform.SetSiblingIndex(siblingIndex);

    card.transform.SetParent(card.GetComponentInParent<Canvas>().transform);
    originalZone = card.CurrentZone;
    card.CurrentZone.RemoveCard(card);
  }

  private void ResetToClone() {
    if(!clone) return;
    originalZone.AddCard(card);
    originalZone = null;
    Destroy(clone);
  }
}