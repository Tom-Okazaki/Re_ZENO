using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MoveEffectPresenter
{
    private CardPrefabView _cardPrefabView;
    private DeckView _deckView;
    private P1_FieldView _p1FieldView;
    private P2_FieldView _p2FieldView;
    private P1_HandView _p1HandView;
    private P2_HandView _p2HandView;

    private P1_HandView _playerHandView;
    private P1_HandView _enemyHandView;


    public MoveEffectPresenter(
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

    public void No1_DumpEnemyCard(Player player, int No)
    {
        if (player == Player.P2)//P2のターンだからP1が効果を受ける
        {
            var dumpCardPrefab = _p1HandView.DumpCardInt(No);
            dumpCardPrefab.transform.DOMove(_p1FieldView.NextField(dumpCardPrefab).position, 2f);
        }
        else
        {
            var dumpCardPrefab = _p2HandView.DumpCardInt(No);
            dumpCardPrefab.transform.DOMove(_p2FieldView.NextField(dumpCardPrefab).position, 2f);
        }
    }

    public void No5_DumpEnemyCard(Player player, WhichCard which)//抽象化が足りない？？？？？？？？？？？？？？？？？
    {
        if(which == WhichCard.None)
        {
            Debug.Log("guard five move");
            return;
        }
        if (player == Player.P1)
        {
            var dumpCardPrefab = _p2HandView.DumpCard(which);
            dumpCardPrefab.transform.DOMove(_p2FieldView.NextField(dumpCardPrefab).position, 2f);
        }
        if(player == Player.P2)
        {
            var dumpCardPrefab = _p1HandView.DumpCard(which);
            dumpCardPrefab.transform.DOMove(_p1FieldView.NextField(dumpCardPrefab).position, 2f);
        }
    }

    public void No7_selectCard(Player player, int selectNo)
    {
        var selectPrefab = _deckView.FindCard(selectNo);
        if (player == Player.P1)
        {
            selectPrefab.transform.DOMove(_p1HandView.AddCard(selectPrefab).position, 2f);
        }
        else
        {
            selectPrefab.transform.DOMove(_p2HandView.AddCard(selectPrefab).position, 2f);
        }
    }

    public void No8_ChangeCard()
    {
        var p1Card = _p1HandView.ReturnCardFor8();
        var p2Card = _p2HandView.ReturnCardFor8();
        p1Card.transform.DOMove(_p2HandView.AddCardFor8(p1Card).position, 2f);
        p2Card.transform.DOMove(_p1HandView.AddCardFor8(p2Card).position, 2f);
    }
}
