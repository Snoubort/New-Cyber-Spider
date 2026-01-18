using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class UICard : MonoBehaviour,
	//IPointerEnterHandler,
	//IPointerExitHandler,
	IPointerDownHandler,
	IPointerUpHandler,
	IBeginDragHandler,
	IDragHandler,
	IEndDragHandler
{
	public CardHand Owner { get; set; }
	public BaseCard CardModel;

	private bool isDragging;

	//// --- Hover ---
	//public void OnPointerEnter(PointerEventData eventData)
	//{
	//	Owner?.OnCardHovered(this);
	//}

	//public void OnPointerExit(PointerEventData eventData)
	//{
	//	Owner?.OnCardUnhovered(this);
	//}

	// --- Press ---
	public void OnPointerDown(PointerEventData eventData)
	{
		isDragging = false;
		Owner?.OnCardPointerDown(this, eventData);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Owner?.OnCardPointerUp(this, isDragging, eventData);
	}

	// --- Drag ---
	public void OnBeginDrag(PointerEventData eventData)
	{
		isDragging = true;
		Owner?.OnCardDragStarted(this);
	}

	public void OnDrag(PointerEventData eventData)
	{
		Owner?.OnCardDragging(this, eventData);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Owner?.OnCardDragEnded(this, eventData);
	}
}
