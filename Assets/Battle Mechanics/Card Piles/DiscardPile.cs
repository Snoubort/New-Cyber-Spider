using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
	public Stack<BaseCard> CardsInDiscardPile = new();

	[SerializeField]
    private DrawPile p_drawPile;
}
