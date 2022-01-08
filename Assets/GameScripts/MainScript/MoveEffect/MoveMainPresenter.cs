using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MoveMainPresenter
{
    private CardPrefabView _cardPrefabView;
    private DeckView _deckView;
    private P1_FieldView _p1FieldView;
    private P2_FieldView _p2FieldView;
    private P1_HandView _p1HandView;
    private P2_HandView _p2HandView;

    public MoveMainPresenter(
        CardPrefabView cardPrefabView,
        DeckView deckView,
        P1_FieldView p1FieldView,
        P2_FieldView p2FieldView,
        P1_HandView p1handView,
        P2_HandView p2HandView)
    {
        _cardPrefabView = cardPrefabView;
        _deckView = deckView;
        _p1FieldView = p1FieldView;
        _p2FieldView = p2FieldView;
        _p1HandView = p1handView;
        _p2HandView = p2HandView;
    }
    public void InstantiateDeck(List<int> deck)
    {
        _deckView.InstantiateDeck(deck,_cardPrefabView);
    }
    
    public void MoveDrawCard(Player player,int cardNo)
    {
        var deckCardPrefab = _deckView.FindCard(cardNo);
        Transform hand;
        if (player == Player.P1) 
        {
            hand = _p1HandView.AddCard(deckCardPrefab);
        }
        else
        {
            hand = _p2HandView.AddCard(deckCardPrefab);
        }
        deckCardPrefab.transform.DOMove(hand.transform.position, 2f);
    }
    
    public void MoveActivateCard(Player player, int cardNo)
    {
        CardPrefabView nowCardPrefab;
        Transform nowFieldPlaceTrans;


        if (player == Player.P1)
        {
            nowCardPrefab = _p1HandView.UseCard(cardNo);
            nowFieldPlaceTrans = _p1FieldView.NextField(nowCardPrefab);
        }
        else
        {
            nowCardPrefab = _p2HandView.UseCard(cardNo);

            nowFieldPlaceTrans = _p2FieldView.NextField(nowCardPrefab);
        }
        var nowCardPrefabTransform = nowCardPrefab.transform;

        nowCardPrefabTransform.DOMove(nowFieldPlaceTrans.position, 2f);
        return;
    }
    //public deck To Tehuda
}
