using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckView : MonoBehaviour
{
    private Transform deckPlaceTrans;

    private List<CardPrefabView> _deckInstantiatedCard = new List<CardPrefabView>();

    private void Start()
    {
        deckPlaceTrans = this.GetComponentInChildren<RectTransform>();
    }

    public void InstantiateDeck(List<int> deck, CardPrefabView cardPrefab) //DeckView
    {
        foreach (var No in deck)
        {
            var card = Instantiate(cardPrefab, deckPlaceTrans);
            _deckInstantiatedCard.Add(card);
            card.SetNo(No);
        }
    }

    public CardPrefabView FindCard(int cardNo)
    {
        foreach (var deckCard in _deckInstantiatedCard)
        {
            if (deckCard.GetNo() == cardNo)
            {
                _deckInstantiatedCard.Remove(deckCard);
                return deckCard;
            }
        }
        Debug.Log("Error Deck View");
        return null;
    }
}
