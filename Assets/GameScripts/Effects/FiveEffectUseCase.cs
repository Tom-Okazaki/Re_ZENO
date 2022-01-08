using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class FiveEffectUseCase
{
    IHand _enemyHand;
    IUsedBox _usedBox;
    IDeck _deck;
    private EffectView _effectView;
    public FiveEffectUseCase(IHand enemyhand, IUsedBox usedbox,IDeck deck, EffectView effectView)
    {
        _enemyHand = enemyhand;
        _usedBox = usedbox;
        _deck = deck;
        _effectView = effectView;
    }

    public void Activate(bool GuardFlug,IObserver<WhichCard> observer,IObserver<Unit> drawEnemyObserver)
    {
        int SelectCard = 0;
        if (GuardFlug == false)
        {
            Debug.Log("five effect Act");
            drawEnemyObserver.OnNext(Unit.Default);
            Observable.FromCoroutine<WhichCard>(observer2 => _effectView.Five_Dump(observer2))
                .Subscribe(which =>
                {
                    if (which == WhichCard.Left)
                    {
                        SelectCard = _enemyHand.RemoveIndex(0);
                    }
                    else
                    {
                        SelectCard = _enemyHand.RemoveIndex(1);
                    }
                    _usedBox.Add(SelectCard);
                    observer.OnNext(which);
                });
        }
        else
        {
            Debug.Log("five effect guarded");
            observer.OnNext(WhichCard.None);
        }
        
    }
}
