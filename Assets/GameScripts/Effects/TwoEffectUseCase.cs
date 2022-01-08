using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TwoEffectUseCase
{
    IHand _EnemyHand;

    private EffectView _effectView;

    public TwoEffectUseCase(IHand EnemyHand, EffectView effectView)
    {
        _EnemyHand = EnemyHand;
        _effectView = effectView;
    }
    public void Activate(bool GuardFlug, IObserver<bool> observer)
    {
        if (GuardFlug == false)
        {
            Observable.FromCoroutine<int>(observer2 => _effectView.Two_SelectNo(observer2))
                .Subscribe(select =>
                {
                    if (select == _EnemyHand.ElementAt(0))
                    {
                        Debug.Log("当たり! :" + select);
                        observer.OnNext(true);
                    }
                    else
                    {
                        Debug.Log("ハズレ! :" + select);
                        observer.OnNext(false);
                    }
                    
                });
        }
        else
        {
            Debug.Log("Two Effect Guarded");
            observer.OnNext(false);
        }
        
    }
}
