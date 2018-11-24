using UnityEngine;
using UnityEngine.EventSystems;

public class Zone: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler {
  public void OnPointerEnter(PointerEventData data) {
    // Debug.Log("Pointer entered me");
  }
  public void OnPointerExit(PointerEventData data) {
    // Debug.Log("Pointer left me");
  }
  public void OnDrop(PointerEventData data) {
    Debug.Log(string.Format("{0} was dropped onto {1}", data.pointerDrag.name, name));
    CardDragHandler dragHandler = data.pointerDrag.GetComponent<CardDragHandler>();
    if(dragHandler != null) {
      dragHandler.isBeingDragged = false;
      dragHandler.transform.SetParent(transform);
    }
  }
}
