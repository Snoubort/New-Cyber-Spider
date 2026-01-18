using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardHand : MonoBehaviour
{
	public int MaxCardsInHand = 0;

	[SerializeField]
	private DrawPile p_drawPile;

	[SerializeField] 
	private DiscardPile p_discardPile;

	[SerializeField]
	private List<BaseCard> p_cardsInHand;

	[SerializeField]
	private CardHandView p_cardHandUi;

	private RectTransform dragProxy;
	[SerializeField] private RectTransform dragLayer;
	[SerializeField] private GameObject dragProxyPrefab;

	[SerializeField]
	private FieldHoverManager p_fieldHoverManager;

	private UICard p_hoveredCard;
	//private UICard p_draggedCard;

	private void Awake()
	{
		p_drawPile.FillPileFromDeck(true, 10);
		GetCardsToHand(2);
		p_cardHandUi.SetCardsFromHandToUI();
	}

	public void GetCardsToHand(int _count)
	{
		if(p_cardsInHand.Count + _count > MaxCardsInHand)
			return;

		for (var i = 0; i < _count; i++)
		{
			p_cardsInHand.Add(p_drawPile.CardsInDrawPile.Dequeue());
		}
	}

	public BaseCard[] GetCardsInHand()
	{
		return p_cardsInHand.ToArray(); 
	}

	public BaseCard GetCardFromHandToMouse(UICard _card)
	{
		p_cardsInHand.Remove(_card.CardModel);

		var canvasGroup = _card.GetComponent<CanvasGroup>();
		canvasGroup.alpha = 0f;              // невидима
		canvasGroup.blocksRaycasts = false;

		p_hoveredCard = _card;

		//p_discardPile.CardsInDiscardPile.Push(_card);
		//p_cardHandUi.SetCardsFromHandToUI();
		return _card.CardModel;
	}

	public void ReturnToHandLastCard()
	{
		//var returnedCard = p_discardPile.CardsInDiscardPile.Pop();

		//p_cardsInHand.Add(returnedCard);
		//p_cardHandUi.SetCardsFromHandToUI();

		var canvasGroup = p_hoveredCard.GetComponent<CanvasGroup>();
		canvasGroup.alpha = 1f;              // невидима
		canvasGroup.blocksRaycasts = true;

		p_cardsInHand.Add(p_hoveredCard.CardModel);

		p_hoveredCard = null;
	}

	//public void OnCardHovered(UICard card)
	//{
	//	p_hoveredCard = card;
	//}

	//public void OnCardUnhovered(UICard card)
	//{
	//	if (p_hoveredCard == card)
	//		p_hoveredCard = null;
	//}

	public void OnCardPointerDown(UICard card, PointerEventData eventData)
	{
		p_fieldHoverManager.HoveredCard = card.CardModel;
		GetCardFromHandToMouse(card);

		dragProxy = Instantiate(dragProxyPrefab, dragLayer)
		.GetComponent<RectTransform>();

		dragProxy.position = eventData.position;
	}

	public void OnCardPointerUp(UICard card, bool wasDragging, PointerEventData eventData)
	{
		if (!p_fieldHoverManager.PlaceElement())
		{
			ReturnToHandLastCard();
		}

		Destroy(dragProxy.gameObject);
	}

	public void OnCardDragStarted(UICard card)
	{
	}

	public void OnCardDragging(UICard card, PointerEventData eventData)
	{
		if (dragProxy != null)
			dragProxy.position = eventData.position;
	}

	public void OnCardDragEnded(UICard card, PointerEventData eventData)
	{
	}

	private bool IsDroppedOnField(PointerEventData eventData)
	{
		// тут проверка drop zone
		return false;
	}

	private void PlayCard(UICard card) { }
}
