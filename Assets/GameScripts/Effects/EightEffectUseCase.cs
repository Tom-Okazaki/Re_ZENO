using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EightEffectUseCase
{
    IHand _myHand;
    IHand _enemyHand;

    public EightEffectUseCase(IHand MyHand, IHand EnemyHand)
    {
        _myHand = MyHand;
        _enemyHand = EnemyHand;
    }
    public void Activate(bool GuardFlug, IObserver<Unit> observer, IObserver<Unit> observerFinish)
    {
        if (GuardFlug == false)
        {
            Debug.Log("eight effect Act");
            var MyCard = _myHand.ElementAt(0);
            var EnemyCard = _enemyHand.ElementAt(0);

            _myHand.RemoveIndex(0);
            _enemyHand.RemoveIndex(0);
            _myHand.Add(EnemyCard);
            _enemyHand.Add(MyCard);
            observer.OnNext(Unit.Default);
        }
        else
        {
            Debug.Log("eight effect Guarded");
            observerFinish.OnNext(Unit.Default);
        }
        
    }
}
