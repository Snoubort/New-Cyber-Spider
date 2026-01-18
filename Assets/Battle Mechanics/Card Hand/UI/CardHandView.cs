using UnityEngine;
using UnityEngine.UI;

public class CardHandView : LayoutGroup
{
	public float CardWidth = 140;

	[SerializeField]
	private int p_maxSpacing = 30;

	[SerializeField]
	private CardHand p_cardHand;

	[SerializeField]
	private GameObject p_cardUiPrefab;

	public override void CalculateLayoutInputHorizontal()
	{
		base.CalculateLayoutInputHorizontal();
		SetCardsPositionsInLayout();
	}

	public override void CalculateLayoutInputVertical() { }

	public override void SetLayoutHorizontal() { }
	public override void SetLayoutVertical() { }

	private void SetCardsPositionsInLayout()
	{
		var cardCount = rectChildren.Count;
		if (cardCount == 0) return;

		var handWidth = rectTransform.rect.width;

		float cardsStartPositionInHand;
		float cardWidthWithSpacigInHand;

		var requiredSpace = cardCount * CardWidth;

		if (requiredSpace <= handWidth)
		{
			var spacing = Mathf.Min(Mathf.Floor((handWidth - requiredSpace) / (cardCount - 1)), p_maxSpacing);
			cardWidthWithSpacigInHand = CardWidth + spacing;
			cardsStartPositionInHand = (handWidth - cardWidthWithSpacigInHand * (cardCount - 1) - CardWidth) / 2;
		}
		else
		{
			float overlap = (cardCount * CardWidth - handWidth) / (cardCount - 1);
			cardWidthWithSpacigInHand = CardWidth - overlap;
			cardsStartPositionInHand = 0;
		}

		for (int i = 0; i < cardCount; i++)
		{
			RectTransform child = rectChildren[i];

			SetChildAlongAxis(child, 0, cardsStartPositionInHand + i * cardWidthWithSpacigInHand, CardWidth);
			SetChildAlongAxis(child, 1, 0, rectTransform.rect.height);
		}
	}

	public void SetCardsFromHandToUI()
	{
		for (int i = transform.childCount - 1; i >= 0; i--)
		{
			Destroy(transform.GetChild(i).gameObject);
		}

		rectChildren.Clear();

		foreach (var card in p_cardHand.GetCardsInHand())
		{
			var newUiCard = Instantiate(p_cardUiPrefab, gameObject.transform);
			if (newUiCard.TryGetComponent<UICard>(out var uiCard))
			{
				uiCard.Owner = p_cardHand;
				uiCard.CardModel = card;
			}
			else
				continue;
			if (newUiCard.TryGetComponent<RectTransform>(out var cardRectTransform))
			{
				cardRectTransform.localScale = Vector3.one;
				cardRectTransform.localRotation = Quaternion.identity;

				cardRectTransform.anchoredPosition = Vector2.zero;

				rectChildren.Add(cardRectTransform);
			}
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;

		if(TryGetComponent<RectTransform>(out var handRectTransfrom))
		{
			Gizmos.DrawWireCube(handRectTransfrom.position, new(handRectTransfrom.rect.width * handRectTransfrom.localScale.x, handRectTransfrom.rect.height * handRectTransfrom.localScale.y));
		}
		
	}
#endif
}
