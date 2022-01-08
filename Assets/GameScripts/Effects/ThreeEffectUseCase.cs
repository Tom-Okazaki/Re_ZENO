using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ThreeEffectUseCase
{
    IHand _EnemyHand;
    private EffectView _effectView;
    public ThreeEffectUseCase(IHand EnemyHand, EffectView effectView)
    {
        _EnemyHand = EnemyHand;
        _effectView = effectView;
    }

    public void Activate(bool GuardFlug, IObserver<Unit> observer)
    {
        if (GuardFlug == true)
        {
            Debug.Log("three effect Guarded");
            observer.OnNext(Unit.Default);
        }
        else
        {
            Debug.Log("three effect Act");
            Observable.FromCoroutine<Unit>(observer2 => _effectView.three_See(observer2, _EnemyHand))
                .Subscribe(_ =>//onComp?
                {
                    observer.OnNext(Unit.Default);//コンプリートで自動で終わってくれる　返り血はここでは　いらないから　調べる
                });
        }
        
    }
}
