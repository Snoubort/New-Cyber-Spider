using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BaseCardDeck", menuName = "Scriptable Objects/BaseCardDeck")]
public class BaseCardDeck : ScriptableObject
{
    [SerializeField]
	public List<DeckCardAmount> CardsInDeck = new();

    public int CardsCount
    {
        get
        {
            var cardsCount = 0;

            foreach (var card in CardsInDeck)
            {
                cardsCount += card.count;
			}
            return cardsCount;
        }
    }
}
