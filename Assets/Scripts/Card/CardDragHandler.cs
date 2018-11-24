using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

  // TODO: There's a bug where sometimes you can drop the card and OnEndDrag doesn't get called :(

  // Used to serve as a proxy wherever it can be dropped.
  private GameObject Proxy;
  // To detect whether or not we're in a valid position.
  // Should be set to false in OnDrop of a valid zone.
  public bool isBeingDragged;
  public void OnBeginDrag(PointerEventData data) {
    isBeingDragged = true;
    ReplaceWithProxy();
    ShouldBlockRaycasts(false);
  }
  public void OnDrag(PointerEventData data) {
    transform.position = Input.mousePosition;
  }
  public void OnEndDrag(PointerEventData data) {
    if(isBeingDragged) RestoreToProxy();
    else Destroy(Proxy);

    ShouldBlockRaycasts(true);
  }

  private void ReplaceWithProxy() {
    int currentIndex = transform.GetSiblingIndex();
    Proxy = new GameObject();
    Proxy.name = transform.name + " (proxy)";
    Proxy.transform.parent = transform.parent;
    transform.SetParent(GetComponentInParent<Canvas>().transform);
    Proxy.transform.SetSiblingIndex(currentIndex);
  }

  private void RestoreToProxy() {
    int currentIndex = Proxy.transform.GetSiblingIndex();
    transform.SetParent(Proxy.transform.parent);
    transform.SetSiblingIndex(currentIndex);
    Destroy(Proxy.gameObject);
  }

  private void ShouldBlockRaycasts(bool value) {
    if(GetComponent<CanvasGroup>() != null) {
      GetComponent<CanvasGroup>().blocksRaycasts = value;
    }
  }
}