using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class P2_HandView : MonoBehaviour
{
     private Transform[] _handPlaceTrans;
    
    private CardPrefabView LeftCard { get; set; }  
    private CardPrefabView RightCard { get; set; }

    void Start()
    {
        _handPlaceTrans = GetComponentsInChildren<RectTransform>();
    }

    public Transform AddCard(CardPrefabView card)
    {
        if (LeftCard == null)
        {
            LeftCard = card;
            return _handPlaceTrans[0];
        }
        else
        {
            RightCard = card;
            return _handPlaceTrans[1];
        }
    }
    
    public Transform AddCardFor8(CardPrefabView card)
    {
        if (LeftCard == null)
        {
            RightCard = card;
            return _handPlaceTrans[1];
            LeftCard = card;
            return _handPlaceTrans[0];
        }
        else
        {
            LeftCard = card;
            return _handPlaceTrans[0];
        }
    }

    public CardPrefabView UseCard(int cardNo)
    {
        if (LeftCard.GetNo() == cardNo)
        {
            var returnCardPrefab = LeftCard;
            LeftCard = RightCard;
            RightCard.transform.DOMove(_handPlaceTrans[0].position, 2f);
            return returnCardPrefab;
        }
        else if (RightCard.GetNo() == cardNo)
        {
            var returnCardPrefab = RightCard;
            RightCard = null;
            return returnCardPrefab;
        }
        else
        {
            Debug.Log("Error P2 Hand View");
            return null;
        }
    }

    public CardPrefabView DumpCard(WhichCard which)
    {
        CardPrefabView returnPrefab;
        if (which == WhichCard.Left)
        {
            returnPrefab = LeftCard;
            LeftCard = null;
        }
        else
        {
            returnPrefab = RightCard;
            RightCard = null;
        }

        return returnPrefab;
    }
    
    public CardPrefabView DumpCardInt(int No)
    {
        CardPrefabView returnPrefab;
        if (LeftCard.GetNo() == No)
        {
            returnPrefab = LeftCard;
            LeftCard = null;
        }
        else
        {
            returnPrefab = RightCard;
            RightCard = null;
        }

        return returnPrefab;
    }
    
    public CardPrefabView ReturnCardFor8()
    {
        CardPrefabView returnCardPrefab = LeftCard;
        if (LeftCard == null)
        {
            returnCardPrefab = RightCard;
        }
        return returnCardPrefab;
    }
}
