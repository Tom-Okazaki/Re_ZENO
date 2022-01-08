using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2_FieldView : MonoBehaviour
{
    private Transform[] _fieldPlaceTrans;

    private List<CardPrefabView> _nowFieldCards = new List<CardPrefabView>();

    //あいてるばしょかえすめそっど
    private void Start()
    {
        _fieldPlaceTrans = this.GetComponentsInChildren<RectTransform>();
    }

    public Transform NextField(CardPrefabView cardPrefab)
    {
        var returnTrans = _fieldPlaceTrans[_nowFieldCards.Count];
        _nowFieldCards.Add(cardPrefab);
        return returnTrans;
    }
}
