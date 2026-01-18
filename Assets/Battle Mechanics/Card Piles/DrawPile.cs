using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawPile : MonoBehaviour
{
    public Queue<BaseCard> CardsInDrawPile = new();

    [SerializeField]
    private BaseCardDeck p_cardDeck;

    [SerializeField]
    private DiscardPile p_discardPile;

	private void Awake()
	{
	}

	public void FillPileFromDeck(bool _isShuffle, int _shuffleCount = 0)
    {
        var cards = new List<BaseCard>();

        foreach (var card in p_cardDeck.CardsInDeck)
        {
            for (var i = 0; i <card.count; i++)
            {
                cards.Add(card.card);
            }
        }

        if (_isShuffle)
        {
            var cardsCount = cards.Count;

            for (var i = 0; i < _shuffleCount; i++)
            {
				var firstCardNumber = Random.Range(0, cardsCount);
                var secondCardNumber = Random.Range(0, cardsCount);

                var cardBuffer = cards[firstCardNumber];
                cards[firstCardNumber] = cards[secondCardNumber];
                cards[secondCardNumber] = cardBuffer;
			}
        }

        CardsInDrawPile = new();
		foreach (var card in cards)
        {
			CardsInDrawPile.Enqueue(card);
		}
	}

    public void RefillPileFromDiscardPile(int _shuffleCount = 0)
    {
        var cards = p_discardPile.CardsInDiscardPile.ToList();
        p_discardPile.CardsInDiscardPile.Clear();

		var cardsCount = cards.Count;

		for (var i = 0; i < _shuffleCount; i++)
		{
			var firstCardNumber = Random.Range(0, cardsCount);
			var secondCardNumber = Random.Range(0, cardsCount);

			var cardBuffer = cards[firstCardNumber];
			cards[firstCardNumber] = cards[secondCardNumber];
			cards[secondCardNumber] = cardBuffer;
		}

		CardsInDrawPile = new();
		foreach (var card in cards)
		{
			CardsInDrawPile.Enqueue(card);
		}
	}
}
